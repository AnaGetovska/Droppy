$(function () {
    $(".gauge").each(function () {
        var guid = $(this).attr('data-value');
        var element = this;

        function doPoll() {
            $.get('/api/value/' + guid, function (data) {
                setValue(data.reading);
                setTimeout(doPoll, 1500);
            });
        }

        function setValue(value){
            var left = $(element).find('.progress-left .progress-bar');
            var right = $(element).find('.progress-right .progress-bar');
            var text = $(element).find('.progress-value div');

            text.text((value * 100).toFixed(2) + "%");

            if (value > 0) {
                if (value <= 0.5) {
                    right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)');
                    left.css('transform', 'rotate(0deg)');
                } else {
                    right.css('transform', 'rotate(180deg)');
                    left.css('transform', 'rotate(' + percentageToDegrees(value - 0.5) + 'deg)');
                }
            }
        }

        doPoll();
    });


    function percentageToDegrees(percentage) {
        return percentage * 360
    }
});

