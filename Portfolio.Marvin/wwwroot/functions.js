
window.scrollToBottom = (element, behavior) => {
    element.scrollTo({
        top: element.scrollHeight,
        behavior: behavior || 'instant'
    });
}

window.loadScript = (url, sync) => {
    
    return new Promise((resolve, reject) => {
       
        const script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = url;
        script.async = !sync;
        
        script.onload = () => { resolve(true); };
        script.onerror = () => { reject(false); };
        
        document.body.appendChild(script);
        
    });
    
}

window.loadScripts = async (urls, sync) => {
    
    const promises = urls.map(url => {
        return loadScript(url, sync);
    });
    
    const results = await Promise.all(promises);
    return results.filter(x => !x).length === 0;
}