
import { useEffect, useState } from 'react'
import './App.css'

function App() {
  const [data, setData] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch('http://localhost:5021/weatherforecast')
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        return response.json();
      })
      .then(data => setData(data))
      .catch(error => setError(error.message));
  }, []);

  return (
    <>
      <h1>Fullstack App</h1>
      <div className="card">
        {error && <p className="error">Error: {error}</p>}
        {data.length === 0 && !error ? <p>Loading...</p> : (
          <ul>
            {data.map((item, index) => (
              <li key={index}>
                {item.date}: {item.summary} ({item.temperatureC}°C)
              </li>
            ))}
          </ul>
        )}
      </div>
    </>
  )
}

export default App
