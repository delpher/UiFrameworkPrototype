function App() {
    const [file, setFile] = useState('no file selected');

    return <Container>
        <Text text={file} />
        <FileInput selectedFile={file} onChange={path => setFile(path)} />
    </Container>
}

<App />
