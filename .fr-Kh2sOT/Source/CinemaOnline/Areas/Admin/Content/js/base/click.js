document.onclick = function (e) {
    e = e || window.event;
    const element = e.target || e.srcElement;

    if (element.href && element.tagName === 'A') {
        notify.loading();
        window.location.href = element.href;
        if (element.href.includes("#")) {
            notify.done();
        }
        return false; // prevent default action and stop event propagation
    } else if (e.srcElement.parentElement.tagName === 'A') {
        notify.loading();
        window.location.href = e.srcElement.parentElement.href;
    }
    return false;
};