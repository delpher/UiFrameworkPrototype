const React = { createElement: (element, props, ...children) => {
        if (Elements.Exposes(element))
            return Framework.createElement(element, props, children);

        Framework.beginComponent();

        return element({...props, children});
    }}

function toUpperCamelCaseNames(props) {
    return Object.getOwnPropertyNames(props).reduce((result, propName) => {
        const upperCamelCaseName = propName.split('').toSpliced(0, 1, propName[0].toUpperCase()).join('');
        result[upperCamelCaseName] = props[propName];
        return result;
    }, {});
}

const useState = Framework.useState;
