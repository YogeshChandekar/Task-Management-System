$(document).ready(function(){
    // set title for the already checked unchecked 
    $('.switch input').parent().attr('title','Disabled');
    $('.switch input:checked').parent().attr('title','Enabled');
    
    // change title on change
    $('.switch input[type="checkbox"]').on('change', function(){
      if($(this).is(":checked")) {
        $(this).parent().attr('title', 'Enabled');
      }else{
        $(this).parent().attr('title', 'Disabled');
      }
    })
});