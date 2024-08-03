const React = (() => {
    return {
        createElement: (element, props, ...children) =>
            Framework.createElement(element, props || {}, children || [])
    }
})();

const useState = Framework.useState;
const useEffect = Framework.useEffect;
