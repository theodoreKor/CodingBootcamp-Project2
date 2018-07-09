var timer = setInterval(randomNumber, 15);
$i  = 0;
function randomNumber() {
  $('span.randomize').each(function() {
    $(this).html(Math.floor((Math.random() * 17895425225245852)))
    $i++;
    if ($i >= 100) {
      var val = $(this).data('number');
      $(this).html(val).removeClass('randomize');
      $i = 0;
    }
  })
}

function clearTimer() { 
  clearInterval(timer);
}