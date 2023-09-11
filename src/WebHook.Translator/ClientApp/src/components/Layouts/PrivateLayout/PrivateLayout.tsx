import { FC, ReactNode } from "react";
import { Link } from "react-router-dom";

// media
import logo from "../../../assets/images/react.svg";

interface IPrivateLayout {
  children: ReactNode;
}

const PrivateLayout: FC<IPrivateLayout> = ({ children }) => {
  return (
    <React.Fragment>
      <div className={"auth"}>
        <div className={"container"}>
          <div className={"row"}>
            <div className={"col"}>
              <Link to={"/"}>
                <img src={logo} alt={"telegram-bot logo"} className={"logo"} />
              </Link>
            </div>
          </div>
          <div className={"row jcc"}>
            <div className={"col"}>
              <div className={"card"}>
                <div className={"card-body"}>
                  <h5 className={"text-primary"}>Добро Пожаловать!</h5>
                  <p className="text-muted">Войдите чтобы продолжить.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default PrivateLayout;
