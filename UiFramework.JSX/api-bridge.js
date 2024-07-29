const React = (() => {
    return {
        createElement: (element, props, ...children) => {
            if (Elements.Exposes(element))
                return Framework.createElement(element, toUpperCamelCaseNames(props), children);

            Framework.beginComponent();
            return element({...props, children});
        }
    }

    function toUpperCamelCaseNames(props) {
        if (!props) return props;
        return Object.getOwnPropertyNames(props).reduce((result, propName) => {
            const upperCamelCaseName = propName.split('').toSpliced(0, 1, propName[0].toUpperCase()).join('');
            return {
                ...result,
                [upperCamelCaseName]: props[propName]
            }
        }, {});
    }
})();

const useState = Framework.useState;
