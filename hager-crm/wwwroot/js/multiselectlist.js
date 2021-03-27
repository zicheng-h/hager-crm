//Customer
$('#btnRight').click(function (e) {
    var selectedOpts = $('#selectedOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move on Customer types.");
        e.preventDefault();
    }

    $('#availOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnLeft').click(function (e) {
    var selectedOpts = $('#availOptions option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move Customer types.");
        e.preventDefault();
    }

    $('#selectedOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

// Contractor
$('#btnRightContractor').click(function (e) {
    var selectedOpts = $('#selectedOptionsCont option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move on Contractor types.");
        e.preventDefault();
    }

    $('#availOptionsCont').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnLeftContractor').click(function (e) {
    var selectedOpts = $('#availOptionsCont option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move Contractor types.");
        e.preventDefault();
    }

    $('#selectedOptionsCont').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

//Vendor
$('#btnRightVendor').click(function (e) {
    var selectedOpts = $('#selectedOptionsVen option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move on Vendor types.");
        e.preventDefault();
    }

    $('#availOptionsVen').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnLeftVendor').click(function (e) {
    var selectedOpts = $('#availOptionsVen option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move Vendor types.");
        e.preventDefault();
    }

    $('#selectedOptionsVen').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnSubmit').click(function (e) {
    $('#selectedOptions option').prop('selected', true);
    $('#selectedOptionsCont option').prop('selected', true);
    $('#selectedOptionsVen option').prop('selected', true);
});

