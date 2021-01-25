using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;

namespace hager_crm.Utils
{
    interface IDBExceptionHandable
    {
        // General database exception handler
        public void HandleDatabaseException(Exception dex, ModelStateDictionary modelState);
    }

    public class DatabaseExceptionHandler
    {

        private DbContext _context;

        public DatabaseExceptionHandler(DbContext context)
        {
            _context = context;
        }

        public void HandleDatabaseException(Exception dex, ModelStateDictionary modelState)
        {
            bool hitExc = false;
            hitExc = TryGetDbUpdateException(dex, modelState) || hitExc;
            hitExc = TryGetRetryLimitExceededException(dex, modelState) || hitExc;
            hitExc = TryGetDbUpdateConcurrencyException(dex, modelState) || hitExc;

            if (!hitExc)
                // Default error if no other handlers
                modelState.AddModelError("", "Error saving your data. Please try again.");
        }

        public bool TryGetDbUpdateConcurrencyException(Exception exc, ModelStateDictionary modelState)
        {
            var dex = exc as DbUpdateConcurrencyException;
            if (dex == null)
                return false;

            modelState.AddModelError("Concurrency Exception",
                            "Your Field was already updated by other user, please refresh the page and save changes again.");
            return true;
        }

        public bool TryGetRetryLimitExceededException(Exception exc, ModelStateDictionary modelState)
        {
            var dex = exc as RetryLimitExceededException;
            if (dex == null)
                return false;

            modelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, later.");
            return true;
        }

        public bool TryGetDbUpdateException(Exception exc, ModelStateDictionary modelState)
        {
            var dex = exc as DbUpdateException;
            if (dex == null)
                return false;

            string dexMessage = dex.GetBaseException().Message;
            // It is right to use actual error code for checking, implement it later with switch case.
            if (dexMessage.Contains("UNIQUE constraint failed:"))
            {
                string[] fields = ExtractExcMessages(dexMessage);
                foreach (var field in fields)
                {
                    modelState.AddModelError(field,
                        $"Unable to save changes, " +
                        $"{(fields.Length > 1 ? "combinations of these fields" : "this field")} " +
                        $"must be unique.");
                }
            }
            else if (dexMessage.Contains("FOREIGN KEY constraint failed"))
                modelState.AddModelError("", "This record is related to other tables. Delete child records first.");
            else
                modelState.AddModelError("", "There was an error during updating database, please try again later.");
            return true;
        }

        public string[] ExtractExcMessages(string message)
        {
            // Could use regular expression but decided to implement it with linq mutations. 
            // In general all SQLite exceptions has same pattern.
            return message
                .Split("'")[1]
                .Trim().Trim('(').Trim(')')
                .Split(",")
                .Select(x => x.Split(".").Last())
                .ToArray();
        }
    }
}
