$(document).ready(function () {
    function matchCustom(params, data) {
        // If there are no search terms, return all of the data
        if ($.trim(params.term) === '') {
          return data;
        }
      
        // Do not display the item if there is no 'text' property
        if (typeof data.text === 'undefined') {
          return null;
        }
      
        // `params.term` should be the term that is used for searching
        // `data.text` is the text that is displayed for the data object
        var searchTerm = params.term.toLowerCase();
        var optionText = data.text.toLowerCase();
      
        if (optionText.indexOf(searchTerm) > -1) {
          var modifiedData = $.extend({}, data, true);
          modifiedData.text += ' (matched)';
      
          // You can return modified objects from here
          // This includes matching the `children` how you want in nested data sets
          return modifiedData;
        }
      
        // Return `null` if the term should not be displayed
        return null;
      }
    
        //Initialize Select2 Elements
    $('.select2').select2({
        matcher: matchCustom
    });
    // use to remove validation for select2 dropdown
    $('.select2').on('select2:select', function (evt) {
        $(this).blur();
    });
});


