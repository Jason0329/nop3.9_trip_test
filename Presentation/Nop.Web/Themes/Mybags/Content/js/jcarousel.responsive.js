(function($) {
    $(function() {
        var jcarousel = $('.jcarousel');

        jcarousel
            .on('jcarousel:reload jcarousel:create', function () {
                var width = jcarousel.innerWidth();
                if (width >= 600){
                    width = width - 60;
					width = width / 5;
                } else if (width >= 550){
					width = width - 30;
                    width = width / 4;
                } else if (width <= 549){
					width = width - 20;
                    width = width / 3;
                }
                jcarousel.jcarousel('items').css('width', width + 'px');
            })
            .jcarousel({
                wrap: 'circular'
            });

        $('.jcarousel-control-prev')
            .jcarouselControl({
                target: '-=1'
            });

        $('.jcarousel-control-next')
            .jcarouselControl({
                target: '+=1'
            });

        $('.jcarousel-pagination')
            .on('jcarouselpagination:active', 'a', function() {
                $(this).addClass('active');
            })
            .on('jcarouselpagination:inactive', 'a', function() {
                $(this).removeClass('active');
            })
            .on('click', function(e) {
                e.preventDefault();
            })
            .jcarouselPagination({
                perPage: 1,
                item: function(page) {
                    return '<a href="#' + page + '">' + page + '</a>';
                }
            });
    });

    $(function () {
        var jcarousel1 = $('.jcarousel-related');

        jcarousel1
            .on('jcarousel:reload jcarousel:create', function () {
                var width = jcarousel1.innerWidth();
              
                if (width >= 600) {
                    width = width - 60;
                    width = width / 4;
                } else if (width >= 451) {
                    width = width - 30;
                    width = width / 2;
                } else if (width <= 450) {
                    width = width;
                    width = width / 1;
                }
                jcarousel1.jcarousel('items').css('width', width + 'px');
            })
            .jcarousel({
                wrap: 'circular'
            });

        $('.jcarousel-related-control-prev')
            .jcarouselControl({
                target: '-=1'
            });

        $('.jcarousel-related-control-next')
            .jcarouselControl({
                target: '+=1'
            });

        $('.jcarousel-pagination')
            .on('jcarouselpagination:active', 'a', function () {
                $(this).addClass('active');
            })
            .on('jcarouselpagination:inactive', 'a', function () {
                $(this).removeClass('active');
            })
            .on('click', function (e) {
                e.preventDefault();
            })
            .jcarouselPagination({
                perPage: 1,
                item: function (page) {
                    return '<a href="#' + page + '">' + page + '</a>';
                }
            });
    });
})(jQuery);