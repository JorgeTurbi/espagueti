//animaciones
document.addEventListener('DOMContentLoaded', () => {
    if ($(".animation-seq").hasClass('hidden'))
        $(".animation-seq").removeClass('hidden');

    const elements_seq = document.querySelectorAll('.animation-seq>:not(.animate)');

    const isScrolledIntoView = (elem) => {
        let docViewTop = window.scrollY;
        let docViewBottom = docViewTop + window.innerHeight;

        let elemTop = $('#' + elem.elem.id).offset().top;
        let elemBottom = elemTop + $('#' + elem.elem.id).height();

        return (elemBottom >= docViewTop && elemTop < docViewBottom);
    };

    const animateOnScroll = () => {
        if (elements_seq.length > 0 && isScrolledIntoView({ elem: elements_seq[0] }, window)) {

            let countms = 0;
            let seq = Array.from(elements_seq).map((v, i) => {
                countms += i > 8 ? 800 : i > 12 ? 450 : 250;
                return { item: v, ms: countms }
            });
            seq.forEach((n, i) => {
                window.setTimeout(() => { n.item.classList.add('animate'); }, n.ms);
            });
        }
    }

    window.addEventListener('scroll', animateOnScroll);    
});