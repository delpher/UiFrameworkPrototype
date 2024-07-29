function App() {
    const [count, setCount] = useState(0);

    return <Container>
        <Text Text={count} />
        <Button Text="Add" OnClick={() => { setCount(count + 1)}} />
    </Container>
}

<App />
