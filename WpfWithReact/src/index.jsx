import ReactReconciler from 'react-reconciler';
import React from 'react';
import {useState, useEffect} from 'react';

const reconciler = ReactReconciler({
    supportsMutation: true,
    createInstance(
        type,
        props,
        rootContainerInstance,
        hostContext,
        internalInstanceHandle
    ) {
        return __WpfHost__.CreateInstance(type, props, rootContainerInstance)
    },
    createTextInstance(
        text,
        rootContainerInstance,
        hostContext,
        internalInstanceHandle,
    ) {
        __WpfHost__.CreateTextInstance(text, rootContainerInstance);
    },
    appendChildToContainer(container, child) {
        __WpfHost__.AppendChildToContainer(container, child);
    },
    appendChild(parent, child) {
        __WpfHost__.AppendChild(parent, child);
    },
    appendInitialChild(parent, child) {
        __WpfHost__.AppendInitialChild(parent, child);
    },
    getRootHostContext() {
    },
    prepareForCommit() {
        __WpfHost__.PrepareForCommit(arguments);
    },
    resetAfterCommit() {
        __WpfHost__.ResetAfterCommit(arguments);
    },
    clearContainer(container) {
        __WpfHost__.ClearContainer(container);
    },
    getChildHostContext(){
    },
    shouldSetTextContent(type, props){
        return __WpfHost__.ShouldSetTextContent(type, props);
    },
    finalizeInitialChildren(instance, type, props, rootContainer, hostContext) {
        return __WpfHost__.FinalizeInitialChildren(instance, type, props, rootContainer, hostContext);
    },
    prepareUpdate(){
        return __WpfHost__.PrepareUpdate(arguments);
    },
    resetTextContent(instance) {
        __WpfHost__.ResetTextContent(instance);
    },
    commitUpdate(instance, hostContext, type, prevProps, nextProps, internalHandle) {
        __WpfHost__.CommitUpdate(instance, hostContext, type, prevProps, nextProps, internalHandle);
    },
    commitTextUpdate(textInstance, prevText, nextText) {
        __WpfHost__.CommitTextUpdate(textInstance, prevText, nextText);
    }
});

const ReactWpf = {
    render(element) {
        const container = reconciler.createContainer(__WpfHost__.Root, false, false);
        reconciler.updateContainer(element, container, null, null);
    }
}

function App() {
    const [text, setText] = useState("some sample text");

    useEffect(() => {
        setTimeout(() => setText("time has passed"), 1000);
    }, []);

    return <text>{text}</text>
}

ReactWpf.render(<App />);
