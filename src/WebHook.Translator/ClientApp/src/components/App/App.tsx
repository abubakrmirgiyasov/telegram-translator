import { useEffect, useState } from "react";

function App() {
  const [count, setCount] = useState(0);

  useEffect(() => {
    fetch("/api/Admin/Get").then((r) => console.log(r));
  }, []);

  return (
    <div>
      <div></div>
    </div>
  );
}

export default App;
