const React = (() => {
    return {
        createElement: (element, props, ...children) =>
            Framework.createElement(element, props || {}, children || []),
        Fragment: Framework.Fragment
    }
})();

const useState = Framework.useState;
const useEffect = Framework.useEffect;
const createContext = Framework.createContext;
const useContext = Framework.useContext;
