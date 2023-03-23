import { useState } from 'react';
import { render } from '@reactunity/renderer';
import './index.scss';

function App() {
  const [searchTerm, setSearchTerm] = useState('');
  const [showSidebar, setShowSidebar] = useState(false);
  const [showSecondSidebar, setShowSecondSidebar] = useState(false);
  const [ setSelectedShelf] = useState(null);
  const shelves = ['Shelf 1', 'Shelf 2', 'Shelf 3', 'Shelf 4', 'Shelf 5', 'Shelf 6', 'Shelf 7', 'Shelf 8'];


  const handleSearch = (e) => {
    console.log('handleSearch called');
    const term = e.target.value;
    setSelectedShelf(null);
    setSearchTerm(term);
    const shelfElems = document.querySelectorAll('.shelf');
    shelfElems.forEach((shelfElem) => shelfElem.ClassList.Remove('selected'));
    if (term) {
      const matchingShelf = Array.from(shelfElems).find((shelfElem) =>
        shelfElem.QuerySelector('h3').TextContent.toLowerCase().includes(term.toLowerCase())
      );
      if (matchingShelf) {
        setSelectedShelf(matchingShelf.QuerySelector('h3').TextContent);
        matchingShelf.ClassList.Add('selected');
        console.log('selected shelf:', matchingShelf);
      }
    }
  };
  return (
    <div>
      <header>
        <h1>Smart Shelf Configurator</h1>
      </header>
        <button className="side-button" onClick={() => setShowSidebar(true)}>=</button>
        <button className="filter-sort" onClick={() => setShowSecondSidebar(true)}>Filter/Sort</button>
      <div className="search-bar">
        <input
          placeholder="Search items..."
          value={searchTerm}
          onChange={handleSearch}
        />
        <button onClick={handleSearch}>Search</button>
      </div>
      <div className="shelf-container">
        {shelves.map((shelf) => (
          <div key={shelf} className="shelf" onClick={() => setSelectedShelf(shelf)}>
            <h3>{shelf}</h3>
          </div>
        ))}
      </div>
      {showSidebar && (
        <div className="sidebar">
          <button className="close-button" onClick={() => setShowSidebar(false)}>X</button>
          <h1>Smart Shelf Configurator</h1>
          <h1>WELCOME</h1>
          <button className="logout-btn">Logout</button>
        </div>
      )}
      {showSecondSidebar && (
  <div className="second-sidebar">
    <button className="close-button" onClick={() => setShowSecondSidebar(false)}>X</button>
    <div className="search-container">
      <input placeholder="Search..." />
      <button>Search</button>
      <select>
        <option value="option1">Option 1</option>
        <option value="option2">Option 2</option>
        <option value="option3">Option 3</option>
      </select>
      <div className="slider-container">
        <label>Slider:</label>
        <div className="slider">
          <div className="slider-track"></div>
          <div className="slider-thumb"></div>
        </div>
        <label>10</label>
      </div>
      <button className="close-btn" onClick={() => setShowSecondSidebar(false)}>Done</button>
    </div>
  </div>
)}

    </div>
  );
}
render(<App />);
