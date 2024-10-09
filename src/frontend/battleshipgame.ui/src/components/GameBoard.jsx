import React, { useState } from 'react';
import GameService from '../services/GameService';
import Cell from './Cell';

const GameBoard = () => {
    const [grid, setGrid] = useState(null);
    const [ships, setShips] = useState([]);
    const [gameResult, setGameResult] = useState(null);
    const [hits, setHits] = useState(0);
    const [remainingHits, setRemainingHits] = useState(0);
    const [isGameOver, setIsGameOver] = useState(false);

    const startGame = () => {
        GameService.startGame()
            .then(() => {
                setIsGameOver(false);
                setHits(0);
                setRemainingHits(0);
                return GameService.getStatus();
            })
            .then(({ data }) => {
                setGrid(data.grid);
                setShips(data.ships);
                setGameResult(data.gameResult);
                setHits(data.hits);
                setRemainingHits(data.remainingHits);
            })
            .catch((error) => {
                console.error("Error starting the game or fetching status:", error);
            });
    };

    const handleAttack = (x, y) => {
        GameService.attack(x, y)
            .then(({ data }) => {
                if (data.result === 'Game over') {
                    setIsGameOver(true);
                    alert('Game over');
                }
                setHits(hits + 1);
                return GameService.getStatus();
            })
            .then(({ data }) => {
                setGrid(data.grid);
                setShips(data.ships);
                setGameResult(data.gameResult);
                setHits(data.hits);
                setRemainingHits(data.remainingHits);
            })
            .catch((error) => {
                console.error("Error attacking or fetching status:", error);
            });
    };

    const isSunkShip = (x, y) => {
        return ships.some(ship =>
            ship.locations.some(loc => loc.x === x && loc.y === y && ship.isSunk)
        );
    };

    return (
        <div className="game-container">
            <div className="row">
                <button onClick={startGame} disabled={(gameResult === 0)}>
                    Start New Game
                </button>
            </div>

            {grid && (
                <div className="game-content">
                    <div className="grid-container">
                        <div className="grid-header">
                            <div className="header-cell"></div>
                            {grid[0]?.map((_, colIndex) => (
                                <div key={colIndex} className="header-cell">X{colIndex}</div>
                            ))}
                        </div>
                        <div className="grid-body">
                            {grid.map((row, rowIndex) => (
                                <div key={rowIndex} className="grid-row">
                                    <div className="row-label">Y{rowIndex}</div>
                                    {row.map((cell, colIndex) => (
                                        <Cell
                                            key={`${rowIndex}-${colIndex}`}
                                            y={rowIndex}
                                            x={colIndex}
                                            shotResult={cell === 1 ? 'Hit!' : cell === 2 ? 'Missed!' : 'empty'}
                                            isDisabled={isGameOver || cell !== 0}
                                            isSunk={isSunkShip(colIndex, rowIndex)}
                                            onClick={handleAttack}
                                        />
                                    ))}
                                </div>
                            ))}
                        </div>
                    </div>

                    <div className="game-status">
                        <h3>Ships Status</h3>
                        <ul>
                            {ships.map((ship, index) => (
                                <li key={index} className={ship.isSunk ? 'sunk' : 'afloat'}>
                                    {ship.name} - {ship.isSunk ? 'Sunk' : 'Afloat'}
                                </li>
                            ))}
                        </ul>
                        <hr />
                        <h3>Game Status</h3>
                        <ul>
                            <li><b>Remaining Hits: {remainingHits}</b></li>
                            <li><b>Game Status: {gameResult === 1 ? 'Won' : gameResult === 2 ? 'Lost' : 'Playing'}</b></li>
                        </ul>
                    </div>
                </div>
            )}
        </div>
    );

};

export default GameBoard;
