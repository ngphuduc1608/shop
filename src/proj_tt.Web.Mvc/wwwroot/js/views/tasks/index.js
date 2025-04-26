(function ($) {
    $(function () {
        var _$taskStateCombobox = $('#TaskStateCombobox');

        _$taskStateCombobox.change(function () {
            location.href = '/Takes?state=' + _$taskStateCombobox.val();
        });
    });
})(jQuery);