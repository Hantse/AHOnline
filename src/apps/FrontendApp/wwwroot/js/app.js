window['navigateToDiv'] = (id) => {
    var divToScroll = document.getElementById(id);
    var top = divToScroll.offsetTop;
    window.scrollTo({
        top: top,
        behavior: 'smooth'
    });
}