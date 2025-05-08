(function ($) {
    $(document).ready(function () {
        const maxStock = parseInt($('#quantity').data('max-stock'));
        const productId = $('#addToCartBtn').data('product-id');
        const $quantityInput = $('#quantity');

        // Prevent non-numeric input
        $quantityInput.on('keypress', function (e) {
            const charCode = e.which || e.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        });

        // Validate on blur
        $quantityInput.on('blur', function () {
            let value = $(this).val();
            if (value === '') {
                $(this).val('1');
                return;
            }

            let numValue = parseInt(value);
            if (numValue > maxStock) {
                $(this).val(maxStock.toString());
            } else if (numValue < 1 || isNaN(numValue)) {
                $(this).val('1');
            }
        });

        // Increment
        $('#incrementBtn').on('click', function () {
            let quantity = parseInt($quantityInput.val());
            if (quantity < maxStock) {
                $quantityInput.val((quantity + 1).toString());
            }
        });

        // Decrement
        $('#decrementBtn').on('click', function () {
            let quantity = parseInt($quantityInput.val());
            if (quantity > 1) {
                $quantityInput.val((quantity - 1).toString());
            }
        });

        // Add to cart button
        $('#addToCartBtn').on('click', function () {
            const quantity = parseInt($quantityInput.val());
            $.ajax({
                url: '/Cart/AddToCart',
                type: 'POST',
                data: {
                    productId: productId,
                    quantity: quantity
                },
                success: function (response) {
                    if (response.success) {
                        abp.notify.success('SavedSuccessfully');
                    } else {
                        abp.notify.error('FailedToSave');
                    }
                },
                error: function () {
                    abp.notify.error('FailedToSave');
                }
            });
        });

        // Buy now button
        $('#buyNowBtn').on('click', function () {
            const quantity = parseInt($quantityInput.val());
            window.location.href = `/Order/Checkout?productId=${productId}&quantity=${quantity}`;
        });
    });
})(jQuery);
