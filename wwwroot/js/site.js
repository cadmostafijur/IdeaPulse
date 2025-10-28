// Global site JavaScript

$(document).ready(function() {
    // Add any global JavaScript functionality here
    
    // Smooth scrolling for anchor links
    $('a[href^="#"]').on('click', function(event) {
        var target = $(this.getAttribute('href'));
        if( target.length ) {
            event.preventDefault();
            $('html, body').stop().animate({
                scrollTop: target.offset().top - 100
            }, 1000);
        }
    });

    // Form validation
    $('form').on('submit', function() {
        const $form = $(this);
        const $required = $form.find('[required]');
        let isValid = true;

        $required.each(function() {
            const $field = $(this);
            if (!$field.val().trim()) {
                $field.addClass('error');
                isValid = false;
            } else {
                $field.removeClass('error');
            }
        });

        if (!isValid) {
            return false;
        }
    });
});

