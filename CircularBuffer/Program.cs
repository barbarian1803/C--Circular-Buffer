using CircularBufferLib;

CircularBuffer<char> buffer = new CircularBuffer<char>(10);

Task taskAdd = Task.Run(() => {
    for (int i = 0; i < 25; i++){
        buffer.Add('a');
    }
    buffer.AddComplete();
});

Task taskGet= Task.Run(() => {
    while(!buffer.IsAddComplete){
        for(int i = 0; i < 7; i++){
            var c = buffer.Get();
            if(c == default(char)){ break; }
            Console.Write(c);
        }
        Console.WriteLine();
    }
});

Task.WaitAll(taskGet, taskAdd);