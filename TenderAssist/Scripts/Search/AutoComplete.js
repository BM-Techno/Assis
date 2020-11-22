function searchAutoCompleteKeywords() {
    var isIndianGlobal = $('#isIndianGlobal').val(); function split(val) { return val.split(/,\s*/); }
    function extractLast(term) { return split(term).pop(); }

    $(".SearchTextBox1").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Common/AutocompleteSearchKeywords',
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                term: extractLast(request.term),
                success: function (data) {
                    response($.map(data, function (item) {
                        var name = item.DisplayText;
                        var value = item.SearchProuctId;
                        return {
                            label: name,
                            value: name,
                            option: value
                        };
                    }));
                }
            });
        },
        search: function () {
            var term = extractLast(this.value);
            if (term.length < 1) {
                return false;
            }
            return true;
        },
        select: function (event, ui) {
            var data = ui.item.label;
            var value = ui.item.option;
            $('.SearchTextBox1').val(data);
            $('#searchTextVal').val(data);
            $('#searchProductID').val(value);


            TenderBy = $('#hdnTendersBy').val() == undefined ? 3 : $('#hdnTendersBy').val();
            FieldId = $('#hdnFieldId').val() == '' || $('#hdnFieldId').val() == undefined ? value : $('#hdnFieldId').val();
            FieldName = $('#hdnFieldName').val() == '' || $('#hdnFieldName').val() == undefined ? data : $('#hdnFieldName').val();
            isWithinSearch = false;

            var IsGlobalSearch = ($('#hdnIsGlobalSearch').val() == 'True') ? true : false;
            if (!IsGlobalSearch) {
                var AdvanceSearch = ($('#hdnIsAdvanceSearch').val() == 'True' || $('#hdnIsAdvanceSearch').val() == 'true') ? true : false;
                if (!AdvanceSearch) {
                    SearchTenders();
                }
            }
            else {
                SearchGlobalTenders();
            }

            return true;
        }
        //appendTo: '#menu-container'
    });

    $(".SearchTextBox2").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Common/AutocompleteSearchKeywords',
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                term: extractLast(request.term),
                success: function (data) {
                    response($.map(data, function (item) {
                        var name = item.DisplayText;
                        var value = item.SearchProuctId;
                        return {
                            label: name,
                            value: name,
                            option: value
                        };
                    }));
                }
            });
        },
        search: function () {
            var term = extractLast(this.value);
            if (term.length < 1) {
                return false;
            }
            return true;
        },
        select: function (event, ui) {
            var data = ui.item.label;
            var value = ui.item.option;
            $('.SearchTextBox2').val(data);
            $('#searchTextVal').val(data);
            $('#searchProductID').val(value);

            TenderBy = $('#hdnTendersBy').val() == undefined ? 3 : $('#hdnTendersBy').val();
            FieldId = $('#hdnFieldId').val() == '' || $('#hdnFieldId').val() == undefined ? value : $('#hdnFieldId').val();
            FieldName = $('#hdnFieldName').val() == '' || $('#hdnFieldName').val() == undefined ? data : $('#hdnFieldName').val();
            isWithinSearch = false;

            var IsGlobalSearch = ($('#hdnIsGlobalSearch').val() == 'True') ? true : false;
            if (!IsGlobalSearch) {
                var AdvanceSearch = ($('#hdnIsAdvanceSearch').val() == 'True' || $('#hdnIsAdvanceSearch').val() == 'true') ? true : false;
                if (!AdvanceSearch) {
                    SearchTenders();
                }
            }
            else {
                SearchGlobalTenders();
            }

            return true;
        }
        //appendTo: '#menu-container'
    });
}