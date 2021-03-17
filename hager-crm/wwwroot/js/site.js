// jQuery scripts to work with filters
$(document).ready(function () {

    $('select.dropdown-country').change(function () {
        let countryId = $(this).find('option:selected').val();
        let dropdownSelector = 'select.dropdown-province.' + $(this).data('link');
        $(dropdownSelector + ' option:selected')
            .each(function() {$(this).data('country') != countryId && countryId != '' && $(this).prop("selected", false)});
        $(dropdownSelector + ' option').each(function () {
            let $item = $(this);
            if (countryId == '' || $item.val() == '' || $item.data('country') == countryId)
                $item.show();
            else
                $item.hide();
        });
    }).trigger('change');
    
    $(".data-sortable").on("click", function () {
        var self = $(this);
        var caret = self.find('span.caret');
        if (caret.hasClass("up")) {
            caret.removeClass("up");
            caret.addClass("down");
            self.data('name')
            window.location = constructQuery(self.data('name'), "ASC");
        }
        else {
            caret.removeClass("down");
            caret.addClass("up");
            window.location = constructQuery(self.data('name'), "DESC");
        }
    });
    $(".data-search-btn").on("click", function () {
        window.location = constructQuery("ID", "ASC", 1);
    });
    $("input.data-filterable").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            window.location = constructQuery("ID", "ASC", 1);
        }
    });
    $(".grid-filter-page").each((i,v) => $(v).prop("href", constructQuery(false, false, $(v).data("page"))));
    // $(".grid-filter-page").on("click", function () {
    //     window.location = constructQuery(false, false, $(this).data("page"));
    // });
    $('#grid-page-size').first().change(function () {
        var pageSize = $('#grid-page-size option:selected').first().val();
        setCookie('gridPageSize', pageSize, 30);
        window.location = constructQuery();
    });
    $(".data-reset-btn").on("click", function () {
        for (var item of $(".data-filterable")) {
            $(item).val("");
        }
        setCookie('gridPageSize', "", 30);
        window.location = FILTER_URL + cType.indexOf(url.get('CType')) == -1 ? '' : '?' + 'CType=' + url.get('CType');
    });
});

// Construct GET query
function constructQuery(orderBy, dir, pageNumber) {
    orderBy = orderBy || url.get('OrderField') || "ID";
    dir = dir || url.get('OrderDir') || "ASC";
    pageNumber = pageNumber || url.get('Page') || 1;
    query = { OrderField: orderBy, OrderDir: dir, Page: pageNumber };

    if (url.get('CType'))
        query.CType = cType.indexOf(url.get('CType')) == -1 ? "All" : url.get('CType');

    for (var item of $(".data-filterable")) {
        $item = $(item);
        if ($item.val()) {
            if ($item.prop("type") === "radio" && !$item.prop("checked"))
                continue;
            query[$item.data('name')] = $item.val();
        }
    }

    return FILTER_URL + '?' + $.param(query);
}

// Cookie Setter
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
