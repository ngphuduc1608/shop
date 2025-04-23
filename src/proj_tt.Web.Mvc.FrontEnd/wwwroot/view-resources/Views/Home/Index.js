(function ($) {
    var _productService = abp.services.app.product,
        l = abp.localization.getSource('proj_tt');



    function loadProducts() {
        var filter = $('#ProductsSearchForm').serializeFormToObject(true);

        _productService.getProductPaged($.extend({}, filter, {
            skipCount: 0,
            maxResultCount: 10,
            sorting: 'CreationTime desc'
        })).done(function (result) {
            var html = '';

            result.items.forEach(function (p) {
                console.log('p:', p);
                html += `
<div class="col">
    <a href="/Product/Details/${p.id}" class="product-item text-decoration-none">
        <div class="box position-relative">
            <div class="a-img">
                <img class="thumb w-100" src="${p.imageUrl}" alt="${p.name}">
            </div>
            <div class="status">
                <img src="https://en.chuu.co.kr/web/upload/custom_153.gif" alt="">
            </div>
            <div class="discount-badge">
                <span>${p.discount}%</span>
            </div>
            <div class="product-description">
                <p class="name">
                    <span>${p.name}</span>
                </p>
                <div class="price-info">
                    <span class="current-price">$${(p.price * (1 - p.discount / 100)).toFixed(2)} USD</span>
                    <span class="original-price">$${p.price} USD</span>
                </div>
            </div>
        </div>
    </a>
</div>`;
            });

            $('.product-list').html(html);
        }).fail(function (error) {
            console.error("Failed to load products:", error);
        });
    }

    function loadBanner() {
        $.ajax({
            url: abp.appPath + 'api/services/app/Banner/GetListBanner',  // API của bạn
            method: 'GET',
            success: function (data) {
                const activeBanners = data.result.filter(b => b.isActive);
                const html = activeBanners.map(b => `
                    <li class="swiper-slide">
                        <a href="${b.link}">
                            <img src="${b.imageUrl}" alt="${b.title}">
                        </a>
                    </li>
                `).join('');
                $('#banner-list').html(html);

                // Initialize Swiper for top-slide-banners
                var swiper = new Swiper("#top-slide-banners", {
                    slidesPerView: 1,
                    spaceBetween: 30,
                    loop: true,
                    autoplay: {
                        delay: 5000,
                        disableOnInteraction: false,
                    },
                    pagination: {
                        el: "#top-slide-banners .swiper-pagination",
                        clickable: true,
                    },
                    navigation: {
                        nextEl: "#top-slide-banners .swiper-button-next",
                        prevEl: "#top-slide-banners .swiper-button-prev",
                    },
                });
            },
            error: function (err) {
                //console.error('Lỗi khi load banner:', err);
            }
        });
    }



    // Gọi khi trang vừa load
    $(function () {
        loadProducts();
        loadBanner();
    });

    //$(document).on('click', '.item', function () {
    //    const link = $(this).data('link');
    //    if (link) {
    //        window.open(link, '_blank');
    //    }
    //});

})(jQuery);
