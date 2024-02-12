from UnityConnector import UnityConnector


class Connector:
    singleton = None

    def __init__(self, on_received: callable = None):
        self.on_received = on_received

        self._ensure_singleton()

        self._connect_to_unity()

    def send_branch(self, branch_json: list[dict]) -> None:
        # send the branch to Unity
        self.unity_connector.send("branch", {"nodes": branch_json})

    def _ensure_singleton(self) -> None:
        # if there is already a instance...
        if Connector.singleton is not None:
            # ...raise an exception
            raise Exception("Connector is already instantiated!")
        # if there is no instance...
        else:
            # ...set this instance as the singleton
            Connector.singleton = self

    def _connect_to_unity(self) -> None:
        # connect to Unity
        self.unity_connector = UnityConnector(
            port_this=50001,
            port_unity=50002,
            ip="127.0.0.1",
            on_timeout=self._on_timeout,
            on_stopped=self._on_stopped,
        )
        self.unity_connector.start_listening(
            on_data_received=self._on_data_received
        )

    def _on_data_received(self, format_name: str, data) -> None:
        if self.on_received != None:
            self.on_received(format_name, data)

    def _on_timeout(self) -> None:
        # if the connection to Unity is lost...
        raise Exception("Connection to Unity lost!")

    def _on_stopped(self) -> None:
        # if the connection to Unity is stopped...
        raise Exception("Connection to Unity stopped!")
