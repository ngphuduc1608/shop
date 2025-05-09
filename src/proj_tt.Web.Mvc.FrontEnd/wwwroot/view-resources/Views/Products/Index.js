(function ($) {
    $(document).ready(function () {
        var selectedCategories = [];
        $("#priceRange").ionRangeSlider({
            type: "double",
            min: 0,
            max: 1000,
            from: parseInt($("#MinPriceInput").val()) || 0,
            to: parseInt($("#MaxPriceInput").val()) || 1000,
            step: 10,
            grid: true,
            prefix: "$",
            onFinish: function (data) {
                $("#MinPriceInput").val(data.from);
                $("#MaxPriceInput").val(data.to);
                $("#filterMinPrice").val(data.from);
                $("#filterMaxPrice").val(data.to);
            }
        });

        $("#MinPriceInput").on('change', function () {
            var value = parseInt($(this).val()) || 0;
            $("#priceRange").data("ionRangeSlider").update({ from: value });
            $("#filterMinPrice").val(value);
        });

        $("#MaxPriceInput").on('change', function () {
            var value = parseInt($(this).val()) || 1000;
            var slider = $("#priceRange").data("ionRangeSlider");
            $("#filterMaxPrice").val(value);
            if (value > slider.options.max) {
                slider.update({ max: value, to: value });
            } else {
                slider.update({ to: value });
            }
        });

        $('.category-checkbox').on('change', function () {
            selectedCategories = [];
            $('.category-checkbox:checked').each(function () {
                selectedCategories.push($(this).val());
            });

            var $form = $('#filterForm');
            $form.find('input[name="categoryIds"]').remove();
            selectedCategories.forEach(function (id) {
                $form.append('<input type="hidden" name="categoryIds" value="' + id + '">');
            });
        });

        $('#filterForm').on('submit', function(e) {
            e.preventDefault();
            selectedCategories = [];
            $('.category-checkbox:checked').each(function () {
                selectedCategories.push($(this).val());
            });
            
            var $form = $(this);
            $form.find('input[name="categoryIds"]').remove();
            selectedCategories.forEach(function (id) {
                $form.append('<input type="hidden" name="categoryIds" value="' + id + '">');
            });
            
            this.submit();
        });


        $('.btn-outline-secondary').on('click', function () {
            $("#priceRange").data("ionRangeSlider").reset();
            $("#MinPriceInput").val('');
            $("#MaxPriceInput").val('');
            $('.category-checkbox').prop('checked', false);
        });
    });
})(jQuery);