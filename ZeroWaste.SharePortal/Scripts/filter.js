$(document).ready(function () {
    bindingQuickSand();
});

var visiblePopover;

function bindingQuickSand() {
    // attempt to call Quicksand when a filter option
    // item is clicked
    //$('#filterOptions li a').click(function (e) {
        // get the action filter option item on page load
        var $filter = $('#filterOptions li.active a').attr('class');
        // get and assign the ourHolder element to the
        // $holder varible for use later
        var $holder = $('ul.ourHolder');

        // clone all items within the pre-assigned $holder element
        var $data = $holder.clone();

        // reset the active class on all the buttons
        //$('#filterOptions li').removeClass('active');

        // assign the class of the clicked filter option
    // element to our $filterType variable
        //$(this).parent().addClass('active');

        var categoryId = $("#categoryId").val();
        if (categoryId == "0") {
            $filter = "all";
        }
        else if (categoryId == "1") {
            $filter = "share";
        }
        else if (categoryId == "2") {
            $filter = "together";
        }
        else if (categoryId == "3") {
            $filter = "borrow";
        }

        if ($filter == 'all') {
            // assign all li items to the $filteredData var when
            // the 'All' filter option is clicked
            var $filteredData = $data.find('li');
        }
        else {
            // find all li elements that have our required $filterType
            // values for the data-type element
            var $filteredData = $data.find('li[data-type=' + $filter + ']');
        }

        // call quicksand and assign transition parameters
        $holder.quicksand($filteredData, {
            duration: 800,
            easing: 'easeInOutQuad'
        }
        , function () { // callback function
            $('.detailPopover').popover(
				{
				    placement: 'bottom',
				    html: true,
				    container: 'body'
				});

            $('.detailPopover').on('click', function (e) {
                // don't fall through
                //alert(0);
                e.stopPropagation();
                var $this = $(this);
                // check if the one clicked is now shown
                if ($this.data('popover').tip().hasClass('in')) {
                    // if another was showing, hide it
                    //alert(1);
                    visiblePopover && visiblePopover.popover('hide');
                    // then store the current popover
                    visiblePopover = $this;
                } else {
                    // if it was hidden, then nothing must be showing
                    //alert(2);
                    visiblePopover = '';
                }
            });
        }

			);
    //    e.preventDefault();
    //});
}