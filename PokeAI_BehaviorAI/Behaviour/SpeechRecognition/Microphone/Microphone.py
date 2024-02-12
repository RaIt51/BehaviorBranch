from pathlib import Path
import wave
import uuid
import pyaudio
from pynput import keyboard
from Behaviour.Config import Config

CHUNK = 8192
FORMAT = pyaudio.paInt16
CHANNELS = 2
RATE = 44100
RECORD_INTERVAL = 0.05


class Microphone:
    def __init__(self, config: Config) -> None:
        self.config = config

        self._audio = pyaudio.PyAudio()

        self._recording = False
        self._frames = []
        self._stream = None

        self._set_wavefile()

    def start_listen_for_key(
        self, key: keyboard.Key, on_recorded: callable
    ) -> None:
        if self.config.verbose > 0:
            print("start listening for key press")

        self._key = key
        self._onRecorded = on_recorded

        # blocking for listening
        with keyboard.Listener(
            on_press=self._on_key_press, on_release=self._on_key_release
        ) as listener:
            listener.join()

    def _on_key_press(self, key: keyboard.Key):
        if (not self._recording) and (key == self._key):
            self._start_recording()

    def _on_key_release(self, key: keyboard.Key) -> bool:
        if (self._recording) and (key == self._key):
            self._stop_recording()

        # not stopping the listener
        return True

    def _start_recording(self) -> None:
        if self.config.verbose > 0:
            print("start recording")

        self._frames = []
        self._stream = self._audio.open(
            format=FORMAT,
            channels=CHANNELS,
            rate=RATE,
            input=True,
            frames_per_buffer=CHUNK,
            stream_callback=self._recording_callback,
        )

        self._recording = True

    def _stop_recording(self) -> None:
        self._recording = False
        self._stream.stop_stream()
        self._stream.close()

        self._write_frames()
        self.wavefile.close()

        if self.config.verbose > 0:
            print("recording stopped")

        # call callback
        self._onRecorded(self.output_file)

        # prepare for next recording
        self._set_wavefile()

    def _write_frames(self) -> None:
        self.wavefile.writeframes(b"".join(self._frames))

    def _recording_callback(self, in_data, frame_count, time_info, status):
        self._frames.append(in_data)
        return (in_data, pyaudio.paContinue)

    def _new_audiofile_name(self) -> str:
        return (
            self.config.audio_directory / (uuid.uuid4().hex + ".wav")
        ).as_posix()

    def _set_wavefile(self):
        self.output_file = self._new_audiofile_name()
        self.wavefile = wave.open(self.output_file, "wb")
        self.wavefile.setnchannels(CHANNELS)
        self.wavefile.setsampwidth(self._audio.get_sample_size(FORMAT))
        self.wavefile.setframerate(RATE)
