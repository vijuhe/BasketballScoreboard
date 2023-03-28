function playAudio(audioElementId) {
    const audioElement = document.getElementById(audioElementId);
    if (audioElement.paused) audioElement.play();
}
