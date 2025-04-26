const container = document.querySelector('.board-grid');

const size = 15;

const matrix = [
    [33, 0, 0, 2, 0, 0, 0, 33, 0, 0, 0, 2, 0, 0, 33],
    [0, 22, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 22, 0],
    [0, 0, 22, 0, 0, 0, 2, 0, 2, 0, 0, 0, 22, 0, 0],
    [3, 0, 0, 22, 0, 0, 0, 2, 0, 0, 0, 22, 0, 0, 3],
    [0, 0, 0, 0, 22, 0, 0, 0, 0, 0, 22, 0, 0, 0, 0],
    [0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0],
    [0, 0, 2, 0, 0, 0, 2, 0, 2, 0, 0, 0, 2, 0, 0],
    [33, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 33],
    [0, 0, 2, 0, 0, 0, 2, 0, 2, 0, 0, 0, 2, 0, 0],
    [0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0],
    [0, 0, 0, 0, 22, 0, 0, 0, 0, 0, 22, 0, 0, 0, 0],
    [3, 0, 0, 22, 0, 0, 0, 2, 0, 0, 0, 22, 0, 0, 3],
    [0, 0, 22, 0, 0, 0, 2, 0, 2, 0, 0, 0, 22, 0, 0],
    [0, 22, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 22, 0],
    [33, 0, 0, 2, 0, 0, 0, 33, 0, 0, 0, 2, 0, 0, 33]
]

const color = {
    0: '#264158',
    1: '#EFEFEF',
    2: '#91AF97',
    3: '#FEDCA0',
    22: '#909EA9',
    33: '#D1401E'
}

for (let i = 0; i < 15; i++) {
    for (let j = 0; j < 15; j++) {
        const cell = document.createElement('div');
        cell.classList.add('cell');
        cell.style.backgroundColor = color[matrix[i][j]];
        container.appendChild(cell);
    }
}


const slotsContainers = document.querySelectorAll('.slots');

slotsContainers.forEach(slots => {
    for (let i = 0; i < 14; i++) {
        const slot = document.createElement('div');
        slot.classList.add('slot');
        slots.appendChild(slot);
    }
});