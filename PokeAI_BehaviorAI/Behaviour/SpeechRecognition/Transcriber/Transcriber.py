from pathlib import Path
from faster_whisper import WhisperModel
from Behaviour.Config import Config
from Behaviour.Utils import Verboser


class Transcriber:
    def __init__(self, config: Config) -> None:
        self.config = config

        self.model = WhisperModel(
            self.config.whisper_model_size, self.config.whisper_device
        )

        self.verboser = Verboser(config)

    def transcribe(self, wav_file_path: Path) -> str:
        self.verboser.print("transcribing...", 1)
        self.verboser.start_timer()

        # transcribe
        segments, _ = self.model.transcribe(
            wav_file_path,
            self.config.language_speech,
            without_timestamps=True,
            beam_size=5,
            # vad_filter=True,
        )
        segments = list(segments)

        # connect only texts
        text = ""
        for segment in segments:
            text += segment.text

        self.verboser.print(f"transcribed: {text}", 1)

        self.verboser.finish_timer("transcription")

        return text

    def _prepare_prompt(self) -> str:
        with open(self.config.whisper_prompt, "r", encoding="utf-8") as f:
            prompt = f.read()

        return prompt
