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


document.addEventListener("DOMContentLoaded", () => {
    const readyButton = document.getElementById("ready-button");
    readyButton.addEventListener("click", () => {
    });
});


let activePlayerId = null;

function fetchActivePlayer() {
    fetch('/Game/GetActivePlayer')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                activePlayerId = data.activePlayer;
            } else {
                console.error("Ошибка при получении активного игрока:", data.message);
            }
        })
        .catch(error => {
            console.error("Ошибка при запросе активного игрока:", error);
        });
}


function toDraggable(element) {
    element.addEventListener("dragover", (e) => {
        e.preventDefault();
    });

    element.addEventListener("drop", (e) => {
        e.preventDefault();

        const dragData = JSON.parse(e.dataTransfer.getData("text/plain"));
        const draggedElement = document.querySelector(`[data-id="${dragData.id}"]`);

        if (!draggedElement || !e.target.classList.contains('slot') && !e.target.classList.contains('board-cell'))
            return;

        if (!canMoveTile(dragData, e.target))
            return;

        resetElementToEmpty(draggedElement);
        fillTargetElement(e.target, dragData);

    });
}

function canMoveTile(dragData, targetElement) {
    if (targetElement.classList.contains("board-cell")) {
        return activePlayerId === null || dragData.owner === activePlayerId;
    } else if (targetElement.classList.contains("slot")) {
        const targetPlayerId = targetElement.dataset.player;

        console.log(dragData.owner, targetPlayerId);
        return String(dragData.owner) === targetPlayerId;
    }
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


// Функция для проверки, можно ли перемещать фишку

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

// Функция для раздачи новых фишек
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


function toDroppable(element) {
    element.setAttribute("draggable", "true");

    // Добавляем обработчики событий Drag and Drop
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


