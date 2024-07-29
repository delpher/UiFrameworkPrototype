function App() {
    const [count, setCount] = useState(0);

    return <Container>
        <Text Text={`Count: ${count}`} />
        <Button Text={'Click me'} OnClick={() => setCount(count + 1)} />
    </Container>
}

<App />
