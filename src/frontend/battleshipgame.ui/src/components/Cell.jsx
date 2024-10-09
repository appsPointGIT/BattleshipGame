import React from 'react';

const Cell = (props) => {
    const getClassName = () => {
        if (props.isSunk) return 'cell sunk';
        if (props.shotResult === 'Hit!') return 'cell hit';
        if (props.shotResult === 'Missed!') return 'cell miss';
        if (props.isDisabled) return 'cell disabled';
        return 'cell empty';
    };

    return (
        <div
            className={getClassName()}
            onClick={() => !props.isDisabled && props.onClick(props.x, props.y)}
        ></div>
    );
};

export default Cell;
