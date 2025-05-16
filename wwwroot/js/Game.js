document.addEventListener("DOMContentLoaded", async () => {
    try {
        const boardCells = document.querySelectorAll(".board-cell");
        const playerSlots = document.querySelectorAll(".slot");

        boardCells.forEach(toDraggable);
        playerSlots.forEach(toDraggable);

        await Promise.all([fetchNewTiles(0), fetchNewTiles(1)]);

        setupReadyButton();
    } catch (error) {
        console.error("Initialization error:", error);
    }
    
});

async function fetchActivePlayer() {
    try {
        const response = await fetch('/Game/GetActivePlayer');
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

        const data = await response.json();
        if (!data.success) {
            console.error("Ошибка при получении активного игрока:", data.message);
            return null;
        }

        return data.active === null ? null : Number(data.active);
    } catch (error) {
        console.error("Ошибка при запросе активного игрока:", error);
        return null;
    }
}

function setupReadyButton() {
    const readyButton = document.getElementById("ready-button");
    if (!readyButton) return;

    readyButton.addEventListener("click", handleReadyClick);
}

async function handleReadyClick() {
    try {
        const validationResult = await validateBoard();
        console.log(validationResult);
        if (!validationResult.success) {
            alert(`Validation failed: ${validationResult.message}`);
            return;
        }

        const switchResult = await switchActivePlayer();
        if (!switchResult.success)
            throw new Error(switchResult.message || "Ошибка смены игрока");

        console.log(switchResult);

        await fetchNewTiles(switchResult.oldActivePlayer);

        updateActivePlayerUI(switchResult.newActivePlayer);
        fixChipsOnBoard();

        alert(`Ход принят! Игрок CHILL_GUY_${switchResult.oldActivePlayer} получил новые фишки. Теперь ход игрока CHILL_GUY_${switchResult.newActivePlayer}`);
    } catch (error) {
        console.error("Error in ready button handler:", error);
    }
}

function fixChipsOnBoard() {
    document.querySelectorAll('.board-cell.active-chip').forEach(chip => {
        chip.classList.remove('active-chip');

        chip.removeAttribute('draggable');

        chip.ondragstart = null;
        chip.onmouseenter = null;
        chip.onmouseleave = null;
    });
}

async function switchActivePlayer() {
    const response = await fetch('/Game/SwitchActivePlayer', {
        method: 'POST',
        headers: {'Content-Type': 'application/json'}
    });

    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

    return await response.json();
}

function updateActivePlayerUI(playerId) {
    document.querySelectorAll('.player-info').forEach(el => {
        el.classList.remove('active-player');
    });

    if (playerId !== null) {
        const activePlayerElement = document.querySelector(`.player-info[data-player-id="${playerId}"]`);
        if (activePlayerElement) {
            activePlayerElement.classList.add('active-player');
        }
    }
}

async function validateBoard() {
    const response = await fetch('/Game/ValidateBoard', {
        method: 'POST',
        headers: {'Content-Type': 'application/json'}
    });
    if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
    return await response.json();
}

async function fetchNewTiles(playerId) {
    try {
        const response = await fetch(`/Game/DealNewTiles?playerId=${playerId}`);
        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

        const data = await response.json();
        if (!data.success || !data.tiles)
            throw new Error(data.message || "Failed to get new tiles");

        drawNewTiles(data.tiles, playerId);
    } catch (error) {
        console.error(`Error fetching tiles for player ${playerId}:`, error);
        throw error;
    }
}

function toDraggable(element) {
    element.addEventListener("dragover", (e) => e.preventDefault());

    element.addEventListener("drop", async (e) => {
        e.preventDefault();
        try {
            const dragData = JSON.parse(e.dataTransfer.getData("text/plain"));
            const draggedElement = document.querySelector(`[data-id="${dragData.id}"]`);

            if (!draggedElement || !['slot', 'board-cell'].some(c => e.target.classList.contains(c)))
                return;

            if (!await canMoveTile(dragData, e.target)) return;

            if (!(draggedElement.classList.contains('slot') && e.target.classList.contains('slot')))
                await sendMoveToServer(draggedElement, e.target);

            resetElementToEmpty(draggedElement);
            fillTargetElement(e.target, dragData);
        } catch (error) {
            console.error("Drop error:", error);
        }
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

    element.addEventListener("dragend", (e) => e.target.classList.remove("dragging"));
}

async function sendMoveToServer(draggedElement, toElement) {
    
    try {
        const response = await fetch('/Game/HandleTileMove', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
                tileId: draggedElement.dataset.id,
                fromType: draggedElement.classList.contains('slot') ? 'slot' : 'board-cell',
                toType: toElement.classList.contains('slot') ? 'slot' : 'board-cell',
                playerId: draggedElement.dataset.owner,
                fromPosition: {
                    row: draggedElement.classList.contains('board-cell') ? draggedElement.dataset.row : null,
                    column: draggedElement.classList.contains('board-cell') ? draggedElement.dataset.col : null
                },
                toPosition: {
                    row: toElement.classList.contains('board-cell') ? toElement.dataset.row : null,
                    column: toElement.classList.contains('board-cell') ? toElement.dataset.col : null
                }
            })
        });

        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

        const data = await response.json();
        if (!data.success) throw new Error(data.message || "Move validation failed");
    } catch (error) {
        console.error("Ошибка при отправке хода:", error);
        throw error;
    }
}

function drawNewTiles(tiles, playerId) {
    const emptySlots = document.querySelectorAll(`.slot[data-player='${playerId}']`);
    tiles.forEach((tile, index) => {
        if (index < emptySlots.length) {
            const slot = emptySlots[index];
            slot.innerHTML = `<p class="value">${tile.Value}</p>
                              <p class="letter">${tile.Letter}</p>`;
            slot.classList.add("chip", "active-chip");
            slot.dataset.id = tile.Id;
            slot.dataset.letter = tile.Letter;
            slot.dataset.value = tile.Value;
            slot.dataset.owner = playerId;
            toDroppable(slot);
        }
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
    target.innerHTML = `<p class="value">${dragData.value}</p>
                      <p class="letter">${dragData.letter}</p>`;
    target.classList.add('chip', 'active-chip');
    toDroppable(target);
    target.dataset.id = dragData.id;
    target.dataset.letter = dragData.letter;
    target.dataset.value = dragData.value;
    target.dataset.owner = dragData.owner;
}