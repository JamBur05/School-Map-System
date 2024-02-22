namespace SchoolMapSystem
{
    class Stack
    {
        private int top;
        private int[] sarray;
        private int maxvalue;

        public Stack(int size) // Constructor, declares stack size and intilises the top to -1.
        {
            sarray = new int[size];
            maxvalue = size;
            top = -1;
        }

        public int PointerValue() // Returns the top pointer value
        {
            return top;
        }

        public int TopValue() // Returns the value of the stack at the top
        {
            return sarray[top];
        }

        public int StackSize() // Returns the size of the stack
        {
            return maxvalue;
        }

        public void Push(int node) // Pushes a node to the stack
        {
            // If the stack isnt full, we can add a new node
            if (isFull() != true)
            {
                top++;
                sarray[top] = node;
            }
        }

        public void Pop() // Pop a node from the stack
        {
            // If the stack isnt empty, we can remove a node
            if (isEmpty() != true)
            {
                sarray[top] = 0;
                top--;
            }
        }

        public bool isFull() // Check if the stack is full
        {
            return top == maxvalue - 1;
        }

        public bool isEmpty() // Check if the stack is empty.
        {
            return top == -1;
        }
    }
}
