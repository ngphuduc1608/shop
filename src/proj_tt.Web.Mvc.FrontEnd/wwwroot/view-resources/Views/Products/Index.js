(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt'),
        source = abp.localization.defaultSourceName;
        _$modal = $('#createModal'),
        _$table = $('#ProductsTable'),
        _$grid = $('#ProductsGrid'),
        currentView = 'grid',
        currentPage = 1,
        pageSize = 15;

    var _permissions = {
        create: abp.auth.isGranted('Pages.Products.Create'),
        edit: abp.auth.isGranted('Pages.Products.Edit'),
        delete: abp.auth.isGranted('Pages.Products.Delete')
    };

    $(function () {
        initFilters();
        loadProducts();
        handleViewToggle();
    });

    function initFilters() {
        // Initialize date range picker
        $('#ProductionDateRange').daterangepicker({
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear'
            }
        });

        $('#ProductionDateRange').on('apply.daterangepicker', function(ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
            loadProducts();
        });

        $('#ProductionDateRange').on('cancel.daterangepicker', function(ev, picker) {
            $(this).val('');
            loadProducts();
        });

        // Search input
        $('#SearchTerm').on('keyup', debounce(function() {
            loadProducts();
        }, 500));

        // Price range
        $('#MinPrice, #MaxPrice').on('change', function() {
            loadProducts();
        });

        // Load categories
        _productService.getAllCategories().done(function(result) {
            var html = `
                <div class="form-check mb-2">
                    <input class="form-check-input" type="radio" name="category" id="cat_all" value="" checked>
                    <label class="form-check-label" for="cat_all">${l('AllCategories')}</label>
                </div>`;
            
            result.forEach(function(category) {
                html += `
                    <div class="form-check mb-2">
                        <input class="form-check-input" type="radio" name="category" id="cat_${category.id}" value="${category.id}">
                        <label class="form-check-label" for="cat_${category.id}">${category.name}</label>
                    </div>`;
            });
            
            $('#CategoryList').html(html);
            
            $('input[name="category"]').on('change', function() {
                loadProducts();
            });
        });

        // Initialize price range slider
        $("#priceRange").ionRangeSlider({
            type: "double",
            min: 0,
            max: 2000000000000,
            from: 0,
            to: 2000000000000,
            step: 1000000,
            grid: true,
            prefix: "VND ",
            onFinish: function (data) {
                $("#MinPriceInput").val(data.from);
                $("#MaxPriceInput").val(data.to);
                loadProducts();
            }
        });

        // Initialize category dropdown
        $('#CategoryDropdownEdit').select2({
            placeholder: l("SelectCategory"),
            allowClear: true,
            width: '100%'
        });

        $('#CategoryDropdownEdit').on('change', function() {
            loadProducts();
        });

        // Handle price input changes
        $('#MinPriceInput, #MaxPriceInput').on('change', function() {
            var min = parseInt($('#MinPriceInput').val()) || 0;
            var max = parseInt($('#MaxPriceInput').val()) || 2000000000000;
            
            $("#priceRange").data("ionRangeSlider").update({
                from: min,
                to: max
            });
            
            loadProducts();
        });
    }

    function loadProducts() {
        var filter = {
            searchTerm: $('#SearchTerm').val(),
            minPrice: $('#MinPrice').val(),
            maxPrice: $('#MaxPrice').val(),
            categoryId: $('input[name="category"]:checked').val(),
            productionDateRange: $('#ProductionDateRange').val()
        };

        _productService.getProductPaged($.extend({}, filter, {
            skipCount: (currentPage - 1) * pageSize,
            maxResultCount: pageSize
        })).done(function (result) {
            if (currentView === 'grid') {
                renderGrid(result.items);
            } else {
                renderTable(result.items);
            }
            renderPagination(result.totalCount);
        });
    }

    function renderGrid(products) {
        var html = '';
        products.forEach(function (product) {
            html += `
<div class="col">
    <div class="card h-100 product-card">
        <div class="position-relative">
            <img src="${product.imageUrl}" class="card-img-top" alt="${product.name}" 
                 onerror="this.src='https://placehold.co/300x300?text=Image+Not+Found'">
            ${product.discount > 0 ? `<span class="badge bg-danger position-absolute top-0 start-0 m-2">-${product.discount}%</span>` : ''}
            <div class="position-absolute top-0 end-0 m-2">
                ${getActionButtons(product)}
            </div>
        </div>
        <div class="card-body">
            <h6 class="card-title text-truncate mb-2">${product.name}</h6>
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <span class="fw-bold text-primary">$${(product.price * (1 - product.discount / 100)).toFixed(2)}</span>
                    ${product.discount > 0 ? `<small class="text-muted text-decoration-line-through ms-2">$${product.price}</small>` : ''}
                </div>
                <small class="text-muted">${product.categoryName}</small>
            </div>
        </div>
    </div>
</div>`;
        });

        _$grid.html(html).removeClass('d-none');
        _$table.addClass('d-none');
    }

    function renderTable(products) {
        var html = '';
        products.forEach(function (product) {
            html += `
<tr>
    <td>
        <img src="${product.imageUrl}" alt="${product.name}" style="width: 50px; height: 50px; object-fit: cover;"
             onerror="this.src='https://placehold.co/300x300?text=Image+Not+Found'">
    </td>
    <td>${product.name}</td>
    <td>
        <span class="fw-bold text-primary">$${(product.price * (1 - product.discount / 100)).toFixed(2)}</span>
        ${product.discount > 0 ? `<small class="text-muted text-decoration-line-through d-block">$${product.price}</small>` : ''}
    </td>
    <td>${product.discount}%</td>
    <td>${product.categoryName}</td>
    <td>${moment(product.productionDate).format('L')}</td>
    <td>${getActionButtons(product)}</td>
</tr>`;
        });

        _$table.find('tbody').html(html);
        _$table.removeClass('d-none');
        _$grid.addClass('d-none');
    }

    function getActionButtons(product) {
        var html = '<div class="btn-group">';
        
        if (_permissions.edit) {
            html += `
                <button type="button" class="btn btn-sm btn-outline-primary" onclick="editProduct(${product.id})">
                    <i class="fas fa-edit"></i>
                </button>`;
        }
        
        if (_permissions.delete) {
            html += `
                <button type="button" class="btn btn-sm btn-outline-danger" onclick="deleteProduct(${product.id})">
                    <i class="fas fa-trash"></i>
                </button>`;
        }
        
        html += '</div>';
        return html;
    }

    function renderPagination(totalCount) {
        var totalPages = Math.ceil(totalCount / pageSize);
        var html = '';
        
        if (totalPages > 1) {
            html += `
                <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
                    <a class="page-link" href="#" data-page="${currentPage - 1}">${l('Previous')}</a>
                </li>`;

            for (var i = 1; i <= totalPages; i++) {
                if (i === 1 || i === totalPages || (i >= currentPage - 2 && i <= currentPage + 2)) {
                    html += `
                        <li class="page-item ${i === currentPage ? 'active' : ''}">
                            <a class="page-link" href="#" data-page="${i}">${i}</a>
                        </li>`;
                } else if (i === currentPage - 3 || i === currentPage + 3) {
                    html += '<li class="page-item disabled"><span class="page-link">...</span></li>';
                }
            }

            html += `
                <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
                    <a class="page-link" href="#" data-page="${currentPage + 1}">${l('Next')}</a>
                </li>`;
        }

        $('#Pagination').html(html);
        $('#TotalCount').text(l('TotalXProducts', totalCount));

        // Bind pagination click events
        $('#Pagination .page-link').on('click', function(e) {
            e.preventDefault();
            var page = $(this).data('page');
            if (page && page !== currentPage) {
                currentPage = page;
                loadProducts();
            }
        });
    }

    function handleViewToggle() {
        $('.btn-group [data-view]').on('click', function() {
            $('.btn-group [data-view]').removeClass('active');
            $(this).addClass('active');
            currentView = $(this).data('view');
            loadProducts();
        });
    }

    function debounce(func, wait) {
        var timeout;
        return function() {
            var context = this, args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(function() {
                func.apply(context, args);
            }, wait);
        };
    }

    // Make functions available globally
    window.editProduct = function(id) {
        abp.ajax({
            url: abp.appPath + 'Product/EditModal?productId=' + id,
            type: 'GET',
            dataType: 'html',
            success: function (content) {
                $('#editModal div.modal-content').html(content);
            },
            error: function (e) { }
        });
    };

    window.deleteProduct = function(id) {
        abp.message.confirm(
            l('ProductDeleteWarningMessage'),
            l('AreYouSure'),
            function (isConfirmed) {
                if (isConfirmed) {
                    _productService.delete(id).done(function () {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        loadProducts();
                    });
                }
            }
        );
    };

    _$form.validate({
        rules: {
            Name: {
                required: true,
                minlength: 3,
                maxlength: 100
            },
            Price: {
                required: true,
                number: true,
                min: 0,
                max:2000000000000
            },
            Discount: {
                number: true,
                min: 0,
                max: 100
            },
            ImageUrl: {
                required: true,
                imageExtension: true,
                filesize: 2 * 1024 * 1024 
            },
            CategoryId: {
                required: true
            },
        },
        messages: {
            Name: {
                required: l("ProductNameRequired"),
                minlength: l("PleaseEnterAtLeastNCharacter"),
                maxlength: l("PleaseEnterNoMoreThanNCharacter")
            },
            Price: {
                required: l("PriceRequired"),
                number: l("PriceMustBeNumber"),
                min: l("PriceMin"),
                max: l("PriceMax")
            },
            Discount: {
                number: l("DiscountMustBeNumber"),
                min: l("DiscountMin"),
                max: l("DiscountMax")
            },
            ImageUrl: {
                required: l("ImageRequired"),
                imageExtension: l("ImageExtensionInvalid"),
                filesize: l("ImageFilesizeMax")
            },
            CategoryId: {
                required: l("CategoryRequired")
            }
        }
    });

    // Thêm phương thức kiểm tra size ảnh
    $.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || (element.files[0].size <= param);
    }, 'Dung lượng ảnh vượt quá giới hạn');

    $.validator.addMethod("imageExtension", function (value, element) {
        if (element.files.length === 0) return false;
        var fileName = element.files[0].name;
        return /\.(jpe?g|png|gif|bmp|webp)$/i.test(fileName);
    }, "Chỉ chấp nhận ảnh định dạng JPG, PNG, GIF, BMP");

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault(); // submit không reload trang

        if (!_$form.valid()) {
            return; // không submit nếu không hợp lệ
        }

        var formElement = _$form[0];
        var formData = new FormData(formElement); // lấy cả input và ảnh
        console.log('discount', formData.get('Discount'));

        if (!formData.get('Discount')) {
            formData.set('Discount', 0);
        }

        abp.ui.setBusy(_$modal); // hiển thị trạng thái loading 

        $.ajax({
            url: abp.appPath + 'Product/Create', // Controller Create
            type: 'POST',
            data: formData,
            processData: false, // không chuyển data thành chuỗi Jquer
            contentType: false, //để jQuery không đặt header Content-Type
            success: function () {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.message.success(l('SuccessfullyRegistered'), l('Success'));
                loadProducts();
            },
            error: function (err) {
                abp.notify.error("Thêm sản phẩm thất bại!");
                console.error(err);
            },
            complete: function () {
                abp.ui.clearBusy(_$modal); // khi hoàn tất , Tắt trạng thái loading dù thành công hay thất bại.
            }
        });
    });

    function ImagePreview(modalSelector) {
        const $modal = $(modalSelector);

        // Preview ảnh 
        $modal.find('#image').on('change', function () {
            var file = this.files[0];
            if (!file) return;
            var reader = new FileReader();

            reader.onload = function (e) {
                $modal.find('#imagePreview').attr('src', e.target.result).show(); // lấy result gắn vào src
            };
            //console.log('reader', reader);

            reader.readAsDataURL(this.files[0]); //chuyển sang dạng base64 và gắn vào src
        });

        // Reset preview ảnh 
        $modal.on('hidden.bs.modal', function () { // sự kiện của bootstrap khi đóng modal
            $modal.find('#imagePreview').attr('src', '#').hide();
            $modal.find('#image').val('');
        });
    }

    ImagePreview('#createModal');
    flatpickr("#createModal #SelectedDate", {
        enableTime: false,
        dateFormat: "Y-m-d",
        locale: "vi" // hoặc "default" nếu không cần tiếng Việt
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        loadProducts();
    });

    $('.btn-clear').on('click', (e) => {
        $('#ProductionDateRange').val('');
        $('#MinPriceInput').val('');
        $('#MaxPriceInput').val('');
        $('#CategoryDropdownEdit').val(null).trigger('change');
        $("#priceRange").data("ionRangeSlider").update({
            from: 0,
            to: 2000000000000
        });
        loadProducts();
    });

    var _$productsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _productService.getProductPaged,
            inputFilter: function () {
                var filter = $('#ProductsSearchForm').serializeFormToObject(true);
                // Add additional filter parameters
                filter.minPrice = $('#MinPriceInput').val();
                filter.maxPrice = $('#MaxPriceInput').val();
                filter.categoryId = $('#CategoryDropdownEdit').val();
                filter.productionDateRange = $('#ProductionDateRange').val();
                
                console.log('Dữ liệu gửi đi:', filter);
                return filter;
            }
        },
        // ... existing DataTable configuration ...
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            loadProducts();
            return false;
        }
    });

})(jQuery);
