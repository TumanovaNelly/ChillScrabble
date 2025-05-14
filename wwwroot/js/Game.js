document.addEventListener("DOMContentLoaded", () => {
    const boardCells = document.querySelectorAll(".board-cell");
    const playerSlots = document.querySelectorAll(".slot");

    boardCells.forEach((cell) => {
        toDraggable(cell);
    })
    playerSlots.forEach((slot) => {
        toDraggable(slot);
    })

    fetchNewTiles(0);
    fetchNewTiles(1);
});



async function fetchActivePlayer() {
    try {
        const response = await fetch('/Game/GetActivePlayer');

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        if (data.success) {
            return data.active === null ? null : Number(data.active);
        } else {
            console.error("Ошибка при получении активного игрока:", data.message);
            return null;
        }
    } catch (error) {
        console.error("Ошибка при запросе активного игрока:", error);
        return null;
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const readyButton = document.getElementById("ready-button");
    readyButton.addEventListener("click", () => {
    });
});


function toDraggable(element) {
    element.addEventListener("dragover", (e) => {
        e.preventDefault();
    });

    element.addEventListener("drop", async (e) => {
        e.preventDefault();

        const dragData = JSON.parse(e.dataTransfer.getData("text/plain"));
        const draggedElement = document.querySelector(`[data-id="${dragData.id}"]`);

        if (!draggedElement || !e.target.classList.contains('slot') && !e.target.classList.contains('board-cell'))
            return;

        const canMove = await canMoveTile(dragData, e.target);
        if (!canMove) 
            return;
            
        
        if (!(draggedElement.classList.contains('slot') && e.target.classList.contains('slot'))) 
            try {
                sendMoveToServer(draggedElement, e.target);
            } catch { return; }
        
        resetElementToEmpty(draggedElement);
        fillTargetElement(e.target, dragData);
    });
}

function toDroppable(element) {
    element.setAttribute("draggable", "true");

    element.addEventListener("dragstart", (e) => {
        e.dataTransfer.setData("text/plain", JSON.stringify({
            id: e.target.dataset.id,
            letter: e.target.dataset.letter,
            value: e.target.dataset.value,
            owner: e.target.dataset.owner
        }));
        e.target.classList.add("dragging");
    });

    element.addEventListener("dragend", (e) => {
        e.target.classList.remove("dragging");
    });
}


function fetchNewTiles(playerId) {
    fetch(`/Game/DealNewTiles?playerId=${playerId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Ошибка HTTP: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (data.success && data.tiles) {
                drawNewTiles(data.tiles, playerId);
            } else {
                console.error("Некорректный ответ сервера:", data);
            }
        })
        .catch(error => {
            console.error("Ошибка при получении данных с сервера:", error);
        });
}

function drawNewTiles(tiles, playerId) {
    const emptySlots = document.querySelectorAll(`.slot[data-player='${playerId}']`);

    tiles.forEach((tile, index) => {
        // Проверяем, есть ли свободный слот
        if (index < emptySlots.length) {
            const slot = emptySlots[index];

            // Обновляем содержимое слота
            slot.innerHTML = `<p>${tile.Letter}</p>`;
            slot.classList.add("chip", "active-chip"); // Добавляем новые классы

            // Добавляем атрибуты для данных фишки
            slot.dataset.id = tile.Id;
            slot.dataset.letter = tile.Letter;
            slot.dataset.value = tile.Value;
            slot.dataset.owner = playerId;

            // Делаем слот перетаскиваемым
            toDroppable(slot);
        }
    });
}


function sendMoveToServer(draggedElement, toElement) {
    fetch('/Game/HandleTileMove', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            tileId: draggedElement.dataset.id,
            fromType: draggedElement.classList.contains('slot') ? 'slot' : 'board-cell',  // 'slot' или 'board-cell'
            toType: toElement.classList.contains('slot') ? 'slot' : 'board-cell',      // 'slot' или 'board-cell'
            playerId: draggedElement.dataset.owner,
            letter: draggedElement.dataset.letter,
            value: draggedElement.dataset.value,
            fromPosition: {
                x: draggedElement.classList.contains('board-cell') ? draggedElement.dataset.x : null,
                y: draggedElement.classList.contains('board-cell') ? draggedElement.dataset.y : null
            },
            toPosition: {
                x: toElement.classList.contains('board-cell') ? toElement.dataset.x : null,
                y: toElement.classList.contains('board-cell') ? toElement.dataset.y : null
            }
        })
    })
        .then(response => response.json())
        .then(data => {
            if (!data.success) {
                console.error("Ошибка при сохранении хода:", data.message);
                throw new Error();
            }
        })
        .catch(error => {
            console.error("Ошибка при отправке хода:", error);
        });
}


async function canMoveTile(dragData, targetElement) {
    const activePlayerId = await fetchActivePlayer();
    
    if (targetElement.classList.contains("board-cell"))
        return activePlayerId === null || Number(dragData.owner) === activePlayerId;
    
    if (targetElement.classList.contains("slot"))
        return String(dragData.owner) === targetElement.dataset.player;

    return false;
}

function resetElementToEmpty(element) {
    element.innerHTML = '';
    element.classList.remove('chip', 'active-chip');
    toDraggable(element);
    delete element.dataset.id;
    delete element.dataset.letter;
    delete element.dataset.value;
    delete element.dataset.owner;
}

function fillTargetElement(target, dragData) {
    target.innerHTML = `<p>${dragData.letter}</p>`;
    target.classList.add('chip', 'active-chip');
    toDroppable(target);
    target.dataset.id = dragData.id;
    target.dataset.letter = dragData.letter;
    target.dataset.value = dragData.value;
    target.dataset.owner = dragData.owner;
}






