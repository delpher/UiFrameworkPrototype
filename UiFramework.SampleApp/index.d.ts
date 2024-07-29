declare function Container(props: any, children: any) : (Function)
declare function Button(props: {text: any, onClick: (Function)}) : (Function)

declare type StateSetter<T> = (sate: T) => void

declare function useState<T>(initialState: T): [state: T, setState: StateSetter<T>]
