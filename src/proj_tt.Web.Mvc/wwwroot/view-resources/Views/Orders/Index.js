$(function () {
    var _orderService = abp.services.app.order;
    var _$modal = $('#OrderCreateModal');
    var _$form = _$modal.find('form');

    // Handle delete button click
    $('.delete-order').click(function (e) {
        var orderId = $(this).data('id');
        
        abp.message.confirm(
            'Are you sure you want to delete this order?',
            'Delete Order',
            function (isConfirmed) {
                if (isConfirmed) {
                    _orderService.deleteOrder(orderId)
                        .done(function () {
                            abp.notify.info('Order deleted successfully');
                            location.reload();
                        });
                }
            }
        );
    });

    // Handle update status button click
    $('.update-status').click(function (e) {
        var orderId = $(this).data('id');
        var statusOptions = [
            { value: 0, text: 'Pending' },
            { value: 1, text: 'Processing' },
            { value: 2, text: 'Shipped' },
            { value: 3, text: 'Delivered' },
            { value: 4, text: 'Cancelled' }
        ];

        abp.message.confirm(
            'Select new status for this order:',
            'Update Order Status',
            function (isConfirmed) {
                if (isConfirmed) {
                    var statusSelect = $('<select class="form-control">');
                    statusOptions.forEach(function (option) {
                        statusSelect.append($('<option>', {
                            value: option.value,
                            text: option.text
                        }));
                    });

                    abp.message.prompt(
                        'Select new status:',
                        'Update Order Status',
                        statusSelect,
                        function (status) {
                            if (status) {
                                _orderService.updateOrderStatus({
                                    orderId: orderId,
                                    status: parseInt(status)
                                }).done(function () {
                                    abp.notify.info('Order status updated successfully');
                                    location.reload();
                                });
                            }
                        }
                    );
                }
            }
        );
    });

    // Handle form submission
    _$form.on('submit', function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var order = _$form.serializeFormToObject();
        _orderService.createOrder(order)
            .done(function () {
                _$modal.modal('hide');
                location.reload();
            });
    });

    // Initialize form validation
    _$form.validate({
        rules: {
            Name: "required",
            Address: "required",
            Phone: "required"
        }
    });

    // Handle modal close
    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
}); 