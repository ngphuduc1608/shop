(function ($) {
    var _cartsService = abp.services.app.cart,
        l = abp.localization.getSource('proj_tt'); // dịch đa ngôn ngữ



    $(document).ready(function () {
        // Tăng số lượng
        $(document).on('click', '.quantity-control button:last-child', function () {
            const $row = $(this).closest('tr');
            const cartItemId = $row.find('.cart-item-checkbox').data('id');
            const $input = $row.find('.quantity-control input');
            let quantity = parseInt($input.val()) + 1;

            $.ajax({
                url: '/Cart/UpdateCartItem',
                type: 'POST',
                data: {
                    cartItemId: cartItemId,
                    quantity: quantity
                },
                success: function (response) {
                    if (response.success) {
                        $input.val(quantity);
                        $row.find('.cart-item-checkbox').data('quantity', quantity);
                        const price = parseFloat($row.find('.cart-item-checkbox').data('price'));
                        const total = price * quantity;
                        $row.find('td:nth-last-child(2)').text(`$${total.toFixed(2)}`);
                        if ($row.find('.cart-item-checkbox').is(':checked')) {
                            updateTotal();
                        }
                    }
                }
            });
        });

        // Giảm số lượng
        $(document).on('click', '.quantity-control button:first-child', function () {
            const $row = $(this).closest('tr');
            const cartItemId = $row.find('.cart-item-checkbox').data('id');
            const $input = $row.find('.quantity-control input');
            let quantity = parseInt($input.val()) - 1;
            if (quantity < 1) return;

            $.ajax({
                url: '/Cart/UpdateCartItem',
                type: 'POST',
                data: {
                    cartItemId: cartItemId,
                    quantity: quantity
                },
                success: function (response) {
                    if (response.success) {
                        $input.val(quantity);
                        $row.find('.cart-item-checkbox').data('quantity', quantity);
                        const price = parseFloat($row.find('.cart-item-checkbox').data('price'));
                        const total = price * quantity;
                        $row.find('td:nth-last-child(2)').text(`$${total.toFixed(2)}`);
                        if ($row.find('.cart-item-checkbox').is(':checked')) {
                            updateTotal();
                        }
                    }
                }
            });
        });

       

        // Xóa khỏi giỏ hàng
        $(document).on('click', '.btn-remove', function () {
            const cartItemId = $(this).closest('tr').find('.cart-item-checkbox').data('id');
                $.ajax({
                    url: '/Cart/RemoveFromCart',
                    type: 'POST',
                    data: { 
                        cartItemId: cartItemId
                    },
                }).done(function (response) {
                    if (response.success) {
                        location.reload();
                    }
                });
        });

        // Chọn/bỏ chọn từng item
        $(document).on('change', '.cart-item-checkbox', function () {
            updateTotal();
        });

        // Chọn tất cả
        $('#selectAll').on('change', function () {
            const checked = $(this).is(':checked');
            $('.cart-item-checkbox').prop('checked', checked);
            updateTotal();
        });

        // Nút thanh toán
        $('#checkoutButton').on('click', function () {
            const selectedItems = $('.cart-item-checkbox:checked').map(function () {
                return $(this).data('id');
            }).get();

            if (selectedItems.length === 0) {
                alert('Vui lòng chọn sản phẩm để thanh toán');
                return;
            }

            window.location.href = `/Order/Checkout?selectedItems=${selectedItems.join(',')}`;
        });

        // Hàm cập nhật tổng tiền
        function updateTotal() {
            let total = 0;
            const checkedItems = $('.cart-item-checkbox:checked');
            checkedItems.each(function () {
                const price = parseFloat($(this).data('price'));
                const quantity = parseInt($(this).data('quantity'));
                total += price * quantity;
            });

            $('#totalAmount').text(`$${total.toFixed(2)} USD`);
            $('#selectedItemCount').text(checkedItems.length);
            $('#checkoutButton').prop('disabled', checkedItems.length === 0);

            // Cập nhật checkbox "select all"
            const allCheckboxes = $('.cart-item-checkbox');
            $('#selectAll').prop('checked', checkedItems.length === allCheckboxes.length && allCheckboxes.length > 0);
        }
    });



})(jQuery)