﻿function App() {
    const [count, setCount] = useState(0);

    return <Container>
        <Text text={count}/>
        <Button text="Add" onClick={() => setCount(count + 1)} />
    </Container>
}

<App />
