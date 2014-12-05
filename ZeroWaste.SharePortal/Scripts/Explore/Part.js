(function() {
    
    var showMoreIncrement = 12;

    // closure variable
    var visiblePopover;

    var $exploreItems;
    
    $(document).ready(function () {
        $exploreItems = $("#exploreItems");

        document.onkeydown = function() {
            if (event.keyCode == 13) {
                return false;
            }
        };

        $("#headerPostcode").bind("textchange", function(event, previousText) {
            var text = $(this).val();
            if (text.length == 4 || text.length == 0) {
                // valid postcode set in hidden fild.
                $("#postcode").val(text);

                emptyItem();
                // trigger a search for listings.
                findListings();
            }
        });

        $("#showmore").bind("click", function () {
            // trigger a search for listings.
            findListings();
        });

        var obj = $("#categoryId");
        $("#filterAll").click(function () {
            if (obj.val() === "0")
                return;

            obj.val("0");
            emptyItem();
            getExploreListingByCategory($(this));
        });
        $("#filterShare").click(function () {
            if (obj.val() === "1")
                return;
            obj.val("1");
            emptyItem();

            getExploreListingByCategory($(this));
        });
        $("#filterTogether").click(function () {
            if (obj.val() === "2")
                return;
            obj.val("2");
            emptyItem();

            $exploreItems.children('[data-catId != 2]').each(function () {
                $(this).remove();
            });

            getExploreListingByCategory($(this));
        });
        $("#filterBorrow").click(function () {
            if (obj.val() === "3")
                return;
            obj.val("3");
            emptyItem();

            $exploreItems.children('[data-catId != 3]').each(function () {
                $(this).remove();
            });

            getExploreListingByCategory($(this));
        });

        //$("#form-search").submit(function() {
        //    formSearch();

        //    return false;
        //});

        $("#q").bind("textchange", function(event, previousText) {
            $("#searchVal").val($(this).val());

            emptyItem();
            formSearch();
        });


        $("#iconSearch").click(function() {
            $(".search-container").toggle();
        });

        //$('#iconSearch').click(function() {
        //    $(this).toggleClass('active');
        //});



        // hide all popovers if any non-popover part of the body is clicked
        $("#header,.span4,#footer").on('click', function () {

            $(".detailPopover").popover('hide');
            visiblePopover = '';
        });
        ////but we don't want to hide the popover if clicking on a tab within the popover
        //$('#popover-tabs a').click(function (e) {
        //    e.stopPropagation();
        //});

        $('.accordion').on('show', function(e) {
            $(e.target).prev('.accordion-heading').find('.accordion-toggle').addClass('active');
        });

        $('.accordion').on('hide', function(e) {
            $(this).find('.accordion-toggle').not($(e.target)).removeClass('active');
        });

        var postcodeVal = $("#headerPostcode").val();
        if (postcodeVal && (postcodeVal.length == 4 || postcodeVal.length == 0)) {
            // valid postcode set in hidden fild.
            $("#postcode").val(postcodeVal);
        }

        //connectPopupClickHandlers();


        // Initialise the popovers.
        $(".detailPopover").popover({
            'container': 'body',
            placement: 'bottom',
            html: true
        });

        // Use event delegation to have only a single click handler on the UL element.
        // http://jqfundamentals.com/chapter/events
        $exploreItems.on('click', '.detailPopover', function (e) {

            // only allow 1 popover at a time
            e.stopPropagation();
            var $this = $(this);
            //$(".detailPopover").popover('hide');
            // check if the one clicked is now shown
            if ($this.data('popover').tip().hasClass('in')) {
                // if another was showing, hide it

                visiblePopover && visiblePopover.popover('hide');
                // then store the current popover
                visiblePopover = $this;
            } else {
                // if it was hidden, then nothing must be showing

                visiblePopover = '';
            }
        });
        // trigger a search for listings.
        //findListings();
    });

    function formSearch() {
        var searchVal = $("#q").val();
        if (searchVal && searchVal != null && searchVal != "") {
            // Update the search term hidden field.
            $("#searchVal").val(searchVal);

            // trigger a search for listings.
            findListings();
        }
    }

    function getExploreListingByCategory(obj) {
        // hide existing popover
        $(".detailPopover").popover('hide');
        visiblePopover = '';
        // trigger a search for listings.
        findListings(function() {
            // Callback to set the active category.
            setCategoryActive(obj);
        });
    }

// Main function for requesting for listings from the server.

    function findListings(callback) {
        // hide existing popover
        $(".detailPopover").popover('hide');
        visiblePopover = '';

        var categoryId = $("#categoryId").val();
        var searchVal = $("#searchVal").val();
        var postcode = $("#postcode").val();
        //var url = "/ListingAjax/GetExploreListings?postcode=" + postcode + "&categoryId=" + categoryId + "&searchVal=" + searchVal + "&maxResults=" + maxResults;

        // Add the loaded elements to the url.
        var loadedItems = new Array();
        $exploreItems.children('li.item').each(function() {
            loadedItems.push($(this).data('key'));
        });

        //url = encodeURIComponent(url)

        $.getJSON('/ListingAjax/GetExploreListings', $.param({ postcode: postcode, categoryId: categoryId, searchVal: searchVal, maxResults: showMoreIncrement, l: loadedItems }, true), function (data) {
            onListingResponse(data);
            if (callback && typeof(callback) == "function") {
                callback();
            }
        });
    }

    function onListingResponse(data) {
        if (data && data != null && data.length > 0) {
            formatExploreListing(data);
        } else {
            emptyItem();
        }
    }

    function emptyItem() {
        // hide existing popover
        $(".detailPopover").popover('hide');
        visiblePopover = '';

        $exploreItems.html("");
        $exploreItems.empty();
        $exploreItems.hide();
    }

    function setCategoryActive(obj) {
        $("#filterOptions li").removeClass("active");
        obj.parent().addClass("active");
    }

    function formatExploreListing(data) {
       
        $.each(data, function(index, item) {
            var popupContent = '&lt;div class=&quot;' + item.CategoryName + 'Popover&quot;&gt;&lt;h2&gt;';
            popupContent += item.ListingName + '&lt;/h2&gt;&lt;img src=&quot;';
            popupContent += item.ListingImageLink + '&quot; alt=&quot;' + item.ListingName + '&quot; width=&quot;200&quot; height=&quot;133&quot;&gt;';
            popupContent += '&lt;ul id=&quot;popover-tabs&quot; class=&quot;nav nav-tabs&quot;&gt;&lt;li class=&quot;active&quot;&gt;&lt;a data-toggle=&quot;tab&quot; class=&quot;popover-tab&quot; href=&quot;#profile&quot;&gt;Profile&lt;/a&gt;&lt;/li&gt;&lt;li class=&quot;&quot;&gt;&lt;a data-toggle=&quot;tab&quot; class=&quot;popover-tab&quot; href=&quot;#connect&quot;&gt;Connect&lt;/a&gt;&lt;/li&gt;&lt;/ul&gt;&lt;div class=&quot;tab-content&quot;&gt;&lt;div class=&quot;tab-pane active&quot; id=&quot;profile&quot;&gt;';
            if (item.AboutGroup != null && item.AboutGroup != "") {
                popupContent += '&lt;p&gt;' + item.AboutGroup + '&lt;/p&gt;';
            }
            popupContent += '&lt;/div&gt;&lt;div class=&quot;tab-pane&quot; id=&quot;connect&quot;&gt;';
            if (item.ListingMessage != null && item.ListingMessage != "") {
                popupContent += '&lt;p&gt;' + item.ListingMessage + '&lt;/p&gt;';
            }

            if (item.Phone != null && item.Phone != "") {
                popupContent += '&lt;h4&gt;Phone:&lt;br&gt;&lt;span&gt;' + item.Phone + '&lt;/span&gt;&lt;/h4&gt;';
            }

            if (item.Email != null && item.Email != "") {
                popupContent += '&lt;h4&gt;Email:&lt;br&gt;&lt;a href=&quot;mailto:' + item.Email + '&quot;&gt;' + item.Email + '&lt;/a&gt;&lt;/h4&gt;';
            }
            if (item.MapAddress != null && item.MapAddress != "") {
                popupContent += '&lt;h4&gt;Address:&lt;br&gt;&lt;span&gt;' + item.MapAddress + '&lt;/span&gt;&lt;/h4&gt;';
            }

            if (item.ListingWebLink != null && item.ListingWebLink != "") {
                popupContent += '&lt;div class=&quot;connectLink&quot;&gt;&lt;a href=&quot;' + item.ListingWebLink + '&quot; target=&quot;_blank&quot;&gt;Connect to us&lt;/a&gt;&lt;/div&gt;&lt;/div&gt;';
            }
            popupContent += '&lt;/div&gt;&lt;/div&gt;';

            var li = '<li class="item ' + item.CategoryName + ' ' + item.IconName + '" data-catId=' + item.CategoryId + ' data-id="id-' + item.index + '" data-key="' + item.ListingId + '" data-type="' + item.CategoryName + ' onclick="var _gaq = _gaq || []; _gaq.push(["_trackEvent", "' + item.CategoryName + '", "' + item.IconName + '", "' + item.ListingName + '"]);">';

            li += '<div data-toggle="popover" class="' + item.CategoryName + ' detailPopover" data-content="' + popupContent + '">';
            li += '<span>' + item.ListingName + '</span>';
            li += '<div class="icon"></div>';
            li += '</div>';
            li += '</li>';
            $exploreItems.append(li);
        });
        $exploreItems.show();
        //$exploreItems.empty();
        //$exploreItems.html(html);

        $(".detailPopover").popover({
            'container': 'body',
            placement: 'bottom',
            html: true
        });


        if (data[0].IsShowMore && (data[0].IsShowMore == true || data[0].IsShowMore == "true")) {
            $("#reveal").show();
        } else {
            $("#reveal").hide();
        }
    }

    ////function connectPopupClickHandlers() {

    ////    //enable popovers - you must initialise the popovers so they can be referenced correctly by all browsers
    ////    $(".detailPopover").popover({
    ////        'container': 'body',
    ////        placement: 'bottom',
    ////        html: true
    ////    });

    ////    ////$exploreItems.on('click', '.detailPopover', function(e) {
            
    ////    ////////})
    ////    ////////// only allow 1 popover at a time
    ////    ////////$('.detailPopover').on('click', function (e) {
    ////    ////    // don't fall through
    ////    ////    e.stopPropagation();
    ////    ////    var $this = $(this);
    ////    ////    //$(".detailPopover").popover('hide');
    ////    ////    // check if the one clicked is now shown
    ////    ////    if ($this.data('popover').tip().hasClass('in')) {
    ////    ////        // if another was showing, hide it

    ////    ////        visiblePopover && visiblePopover.popover('hide');
    ////    ////        // then store the current popover
    ////    ////        visiblePopover = $this;
    ////    ////    } else {
    ////    ////        // if it was hidden, then nothing must be showing

    ////    ////        visiblePopover = '';
    ////    ////    }
    ////    ////});
    ////}
}())