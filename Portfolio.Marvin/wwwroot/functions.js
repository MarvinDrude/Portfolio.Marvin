
window.scrollToBottom = (element, behavior) => {
    console.log("a");
    element.scrollTo({
        top: element.scrollHeight,
        behavior: behavior || 'instant'
    });
}
