(function() {
    var map = null;
    var markersArray = new Array();

    var lat = -34.932829;
    var lng = 138.603813;
    var categoryId = 0;
    var zoomLevel = 14;
    var postcode = "5000";

    var loadListingsTimer = null;

    var infoBox = null;

    var tabClickHandlers = [];

    $(document).ready(function() {
        document.onkeydown = function() {
            if (event.keyCode == 13) {
                return false;
            }
        };


        $("#zoomLevel").val(zoomLevel);

        $("#headerPostcode").bind("textchange", function() {
            var text = $(this).val();
            if (text.length == 4) {
                // We have a 4 digit postcode, so do a HTTP GET for the lat log for the center of the postcode area.
                centerOnPostcode(text);
            } else if (text.length == 0) {
                // Do a HTTP POST to clear the session variable for postcode.
                var url = "/ListingAjax/GetLatAndLngByPostcode?postcode=" + text + "&random=" + new Date();
                $.ajax({
                    type: "post",
                    async: true,
                    url: url,
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    cache: false
                });
            }
        });

        //$("#iconSearch").click(function () {
        //    $(".search-container").toggle();
        //});

        //$('#iconSearch').click(function () {
        //    $(this).toggleClass('active');
        //});

        $("#filterAll").parent().addClass('active');

        $("#filterOptions li a").click(function() {
            var obj = $(this);
            filterClick(obj);
            loadMapListings(map);
        });

        initGoogleMap("map_canvas", lat, lng, zoomLevel, google.maps.MapTypeId.ROADMAP, "4392", 250, null, categoryId);

        var postcodeVal = $("#headerPostcode").val();
        if (postcodeVal && postcodeVal.length == 4) {
            centerOnPostcode(postcodeVal);
        }

        //$("#form-search").submit(function() {
        //    formSearch();

        //    return false;
        //});

        $("#q").bind("textchange", function(event, previousText) {
            $("#searchVal").val($(this).val());
            formSearch();
        });
    });

    function centerOnPostcode(postcode) {
        var url = "/ListingAjax/GetLatAndLngByPostcode?postcode=" + postcode + "&random=" + new Date();
        $.getJSON(url, function(data) {
            if (data && data.lat != null && data.lng != null) {
                lat = data.lat;
                lng = data.lng;
                var latlng = new google.maps.LatLng(lat, lng);
                map.panTo(latlng);
            }
        });
    }

    function filterClick(obj) {
        $('#filterOptions li').removeClass('active');
        obj.parent().addClass('active');

        var filterType = obj.attr("class");
        if (filterType == "all") {
            categoryId = 0;
        } else if (filterType == "share") {
            categoryId = 1;
        } else if (filterType == "together") {
            categoryId = 2;
        } else if (filterType == "borrow") {
            categoryId = 3;
        }

        $("#categoryId").val(categoryId);
    }

    function initGoogleMap(canvas, lat, log, zoomLevel, mapType) {
        $(window).unbind("resize");
        var latlng = new google.maps.LatLng(lat, log);
        var mapOptions = {
            zoom: zoomLevel,
            center: latlng,
            styles: [{ stylers: [{ "visibility": "on" }, { "saturation": -80 }, { "lightness": 20 }] }],
            mapTypeId: mapType
        };

        map = new google.maps.Map(document.getElementById(canvas), mapOptions);

        var infoboxOptions = {
            disableAutoPan: false,
            maxWidth: 0,
            pixelOffset: new google.maps.Size(-110, 0),
            zIndex: null,
            boxStyle: {
                width: "220px"
            },
            closeBoxMargin: "13px 2px 2px 2px",
            closeBoxURL: "http://www.google.com/intl/en_us/mapfiles/close.gif",
            infoBoxClearance: new google.maps.Size(1, 1),
            pane: "floatPane",
            enableEventPropagation: true
        };

        infoBox = new InfoBox(infoboxOptions);
        
        google.maps.event.addListener(map, "bounds_changed", function() {
            handleMapExtentsChanged(map);
        });

        //google.maps.event.addListener(map, "click", function() {
        //    infoBox.close();
        //});
    }

    function handleMapExtentsChanged(map) {
        // Check if there is already a timer awaiting to timeout.
        if (loadListingsTimer != null) {
            // There is, so clear hte timer.
            clearTimeout(loadListingsTimer);
        }

        // Set a new timer to wait to timeout.
        loadListingsTimer = setTimeout(function() {
            // Clear the timer reference now that it has timed out.
            loadListingsTimer = null;
            // Timer has timed out, so load trigger listings load.
            loadMapListings(map);
        }, 100);
    }

    function loadMapListings(map) {
        // Get the bounds of the visible map region.
        var bounds = map.getBounds();

        var top = bounds.getNorthEast().lat();
        var right = bounds.getNorthEast().lng();
        var bottom = bounds.getSouthWest().lat();
        var left = bounds.getSouthWest().lng();
        
        // Query for listings in that visible region.
        var url = "/GoogleMap/GetMakers?top=" + top + "&right=" + right + "&bottom=" + bottom + "&left=" + left + "&categoryId=" + categoryId + "&random=" + new Date();
        $.getJSON(url, function(data) {
            if (markersArray) {
                for (i in markersArray) {
                    markersArray[i].setMap(null);
                }
            }
            if (data && data != null && data != "") {
                markersArray = new Array();
                $.each(data, function(i, item) {
                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(item.latitude, item.longitude),
                        map: map,
                        title: item.title,
                        icon: item.icon
                    });

                    markersArray.push(marker);

                    (function(marker) {
                        google.maps.event.addListener(marker, 'click', function () {
                            // Remove the tab click handlers that have been added to the map.
                            for (var k = 0; k < tabClickHandlers.length; k++) {
                                google.maps.event.removeListener(tabClickHandlers[i]);
                            }

                            tabClickHandlers.length = 0;
                            
                            var boxText = document.createElement("div");
                            boxText.className = "popover-content";
                            boxText.innerHTML = item.pop_content;
                            
                            if (infoBox.closed == null || infoBox.closed) {
                                infoBox.setContent(boxText);
                                infoBox.open(map, marker);
                                
                                var tabs = boxText.getElementsByClassName('popover-tab');
                                for (var j = 0; j < tabs.length; j++) {
                                    var tab = tabs[j];
                                    tabClickHandlers.push(google.maps.event.addDomListener(tab, "click", openTab));
                                }
                            } else {
                                infoBox.close();
                            }
                        });
                    })(marker);
                });
            }
        });
    }

    function openTab(e) {
        e.preventDefault();
        $(this).tab('show');
    }

    function formSearch() {
        var maxResults = $("#maxResults").val();
        var searchVal = $("#q").val();
        $("#searchVal").val(searchVal);

        loadMapListings(map);
    }
})();