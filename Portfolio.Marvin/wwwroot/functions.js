
window.scrollToBottom = (element, behavior) => {
    element.scrollTo({
        top: element.scrollHeight,
        behavior: behavior || 'instant'
    });
}

window.loadScript = (url, sync, isModule) => {
    
    return new Promise((resolve, reject) => {
       
        const script = document.createElement('script');
        script.type = isModule ? 'module' : 'text/javascript';
        script.src = url;
        script.async = !sync;
        
        script.onload = () => { resolve(true); };
        script.onerror = () => { reject(false); };
        
        document.body.appendChild(script);
        
    });
    
}

window.loadScripts = async (urls, sync, isModule) => {
    
    const promises = urls.map(url => {
        return loadScript(url, sync, isModule);
    });
    
    const results = await Promise.all(promises);
    return results.filter(x => !x).length === 0;
    
}