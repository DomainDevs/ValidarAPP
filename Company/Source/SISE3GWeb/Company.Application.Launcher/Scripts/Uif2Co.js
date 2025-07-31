(function ($) {
    $.fn.extend({
        UifValidate: function () {

            this.each(function () {
                return true;
            });
        }
    });
})(jQuery);

(function ($) {
    $.fn.HideColums = function (option) {
        var defaults = {
            colums: [],
            control: ""
        }
        var defaults = $.extend(defaults, option)
        var elem = $(defaults.control);
        var oID = $(elem).attr("id");
        $(this).data('defaults', defaults);
        if ($(this).data('defaults') == null) {
            return;
        }
        for (var i = 0; i < $(this).data('defaults').colums.length; i++) {
            var k = $(this).data('defaults').colums[i] + 1;
            $('#' + oID + ' thead th:eq(' + $(this).data('defaults').colums[i] + ')').hide();
            $('#' + oID + ' tbody td:nth-child(' + k + ')').hide();
        }
        this.each(function () {
            
            if (defaults.length === 0) {
                var colum = $(elem).DataTable().columns;
                $(elem).DataTable().on('draw', function () {
                    var i = 0;
                    for (i = 1; i <= defaults.colums.length +1; i++) {
                        $('#table' + oID + ' tbody td:nth-child(' + i + ')').hide();
                    };
                    for (i = 0; i <= defaults.colums.length; i++) {
                        $('#table' + oID + ' thead th:eq(' + i + ')').hide();
                    }
                });


            }
            else {
                $(this).DataTable().on('draw', function () {
                    for (var i = 0; i < $(this).data('defaults').colums.length; i++) {
                        var k = defaults.colums[i] + 1;
                        $('#' + oID + ' thead th:eq(' + defaults.colums[i] + ')').hide();
                        $('#' + oID + ' tbody td:nth-child(' + k + ')').hide();
                    }
                });
            }

        });
        //return this;
    }
    $.fn.UifDataTable.ShowColums = function (option) {

        this.each(function () {
            return true;
        });
        return this;
    }

})(jQuery)


$.fn.UifDataTable.defaults = {
    colums: [],
    control: ""
};


(function ($) {
    $('#btn-sup-access').click(function () {
        location.href = rootPath + "common/main/main?type=4000";
    });
})(jQuery)

function getAttributes($node, indexBase) {
    var attrs = {}; 
    $.each($node, function (index, attribute) {
        if (attribute == indexBase) {
            attrs = index;
            return;
        }
    });

    return attrs;
}

