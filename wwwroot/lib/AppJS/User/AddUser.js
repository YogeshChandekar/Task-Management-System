$(document).ready(function () {
    $('#isActiveDirectory').val(0);
    $('#UserRegisterOrNot').change(function () { //change click to hidden
        if ($(this).is(':checked')) { //check if checkbox is checked
            $('#IsRegwithServer').val(1);
        } else {
            $('#IsRegwithServer').val(0);
        }
    })

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
    $('.select2').on('select2:select', function (evt) {
        $(this).blur();
    });
    // use to remove validation for select2 dropdown
    $('.select2').on('select2:select', function (evt) {
        $(this).blur();
    });

    $("#appAddUser").on("submit", function (e) {
        e.preventDefault();//stop submit event
        var self = $(this);//this form
        let isValid = self.valid();
        if (isValid) {
            $('.loading').show();
            $("#appAddUser").off("submit");//need form submit event off.
            self.unbind('submit');
            self.submit();//submit form
        }
    });

    $('#emailforAD').change(function () {
        $('#Email').val($(this).val());
        // also show display name if attached with AD user
    })
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
});