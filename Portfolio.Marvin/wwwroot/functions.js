
window.scrollToBottom = (element, behavior) => {
    element.scrollTo({
        top: element.scrollHeight,
        behavior: behavior || 'instant'
    });
}
